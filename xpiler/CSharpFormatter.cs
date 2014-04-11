﻿// Copyright (c) 2013 Jae-jun Kang
// See the file COPYING for license details.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace x2
{
    class CSharpFormatter : OutputFormatter
    {
        private const string Extension = ".cs";
        
        public override string Description { get { return "C#"; } }

        public override bool Format(Document doc, string outDir)
        {
            try
            {
                var context = new CSharpFormatterContext()
                {
                    Doc = doc,
                    Target = Path.Combine(outDir, doc.BaseName + Extension)
                };
                using (var writer = new StreamWriter(context.Target))
                {
                    context.Out = writer;
                    FormatHead(context);
                    FormatBody(context);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        private void FormatHead(CSharpFormatterContext context)
        {
            var @out = context.Out;
            @out.WriteLine("// auto-generated by xpiler");
            @out.WriteLine();
            @out.WriteLine("using System;");
            @out.WriteLine("using System.Text;");
            @out.WriteLine();
            @out.WriteLine("using x2;");
            @out.WriteLine();

            foreach (var reference in context.Doc.References)
            {
                reference.Format(context);
            }
        }

        private void FormatBody(CSharpFormatterContext context)
        {
            var @out = context.Out;
            if (!String.IsNullOrEmpty(context.Doc.Namespace))
            {
                @out.WriteLine("namespace {0}", context.Doc.Namespace.Replace('/', '.'));
                @out.WriteLine("{");
                context.Indent();
            }
            var leading = true;
            foreach (var def in context.Doc.Definitions)
            {
                if (leading)
                {
                    leading = false;
                }
                else
                {
                    context.Out.WriteLine();
                }
                def.Format(context);
            }
            if (!String.IsNullOrEmpty(context.Doc.Namespace))
            {
                context.Unindent();
                @out.WriteLine("}");
            }
        }

        public override bool IsUpToDate(string path, string outDir)
        {
            var baseName = Path.GetFileNameWithoutExtension(path);
            var target = Path.Combine(outDir, baseName + Extension);
            return File.Exists(target) &&
                   File.GetLastWriteTime(target) >= File.GetLastWriteTime(path);
        }
    }

    class CSharpFormatterContext : FormatterContext
    {
        private const string Tab = "    ";
        private int baseIndentation = 0;

        private static readonly Dictionary<string, string> nativeTypes;
        private static readonly Dictionary<string, string> defaultValues;

        static CSharpFormatterContext()
        {
            nativeTypes = new Dictionary<string, string>();
            nativeTypes.Add("bool", "bool");
            nativeTypes.Add("byte", "byte");
            nativeTypes.Add("int8", "sbyte");
            nativeTypes.Add("int16", "short");
            nativeTypes.Add("int32", "int");
            nativeTypes.Add("int64", "long");
            nativeTypes.Add("float32", "float");
            nativeTypes.Add("float64", "double");
            nativeTypes.Add("string", "string");
            nativeTypes.Add("datetime", "DateTime");
            nativeTypes.Add("list", "Sequence");
            //nativeTypes.Add("map", "MapCell");

            defaultValues = new Dictionary<string, string>();
            defaultValues.Add("bool", "false");
            defaultValues.Add("byte", "0");
            defaultValues.Add("int8", "0");
            defaultValues.Add("int16", "0");
            defaultValues.Add("int32", "0");
            defaultValues.Add("int64", "0");
            defaultValues.Add("float32", ".0f");
            defaultValues.Add("float64", ".0f");
            defaultValues.Add("datetime", "new DateTime(0)");
            defaultValues.Add("string", "");
        }

        public string Target { get; set; }

        public override void FormatReference(Reference reference)
        {
            Indent(0); Out.WriteLine("using {0};", reference.Target);
            Out.WriteLine();
        }

        public override void FormatConsts(ConstsDef def)
        {
            if (nativeTypes.ContainsKey(def.Type))
            {
                def.NativeType = nativeTypes[def.Type];
            }
            else
            {
                return;
            }

            PreprocessConsts(def);

            Indent(0); Out.WriteLine("public static class {0}", def.Name);
            Indent(0); Out.WriteLine("{");
            foreach (var constant in def.Constants)
            {
                Indent(1);
                Out.Write("public const {0} {1}", def.NativeType, constant.Name);
                if (!String.IsNullOrEmpty(constant.Value))
                {
                    Out.Write(" = {0};", constant.Value);
                }
                Out.WriteLine();
            }
            Indent(0); Out.WriteLine("}");
        }

        public override void FormatCell(CellDef def)
        {
            def.BaseClass = def.Base;
            if (String.IsNullOrEmpty(def.BaseClass))
            {
                def.BaseClass = (def.IsEvent ? "Event" : "Cell");
            }
            PreprocessProperties(def);

            Indent(0); Out.WriteLine("public class {0} : {1}", def.Name, def.BaseClass);
            Indent(0); Out.WriteLine("{");
            Indent(1); Out.WriteLine("new protected static readonly Tag tag;");
            Out.WriteLine();
            if (def.IsEvent)
            {
                Indent(1); Out.WriteLine("new public static int TypeId { get { return tag.TypeId; } }");
                Out.WriteLine();
            }
            FormatFields(def);
            if (def.HasProperties)
            {
                Out.WriteLine();
            }
            FormatProperties(def);
            if (def.HasProperties)
            {
                Out.WriteLine();
            }
            FormatMethods(def);
            Indent(0); Out.WriteLine("}");
        }

        private void FormatFields(CellDef def)
        {
            foreach (var property in def.Properties)
            {
                Indent(1);
                Out.WriteLine("private {0} {1};", property.NativeType, property.NativeName);
            }
        }

        private void FormatProperties(CellDef def)
        {
            var leading = true;
            foreach (var property in def.Properties)
            {
                if (leading)
                {
                    leading = false;
                }
                else
                {
                    Out.WriteLine();
                }
                Indent(1); Out.WriteLine("public {0} {1}", property.NativeType, property.Name);
                Indent(1); Out.WriteLine("{");
                Indent(2); Out.WriteLine("get {{ return {0}; }}", property.NativeName);
                Indent(2); Out.WriteLine("set");
                Indent(2); Out.WriteLine("{");
                Indent(3); Out.WriteLine("fingerprint.Touch(tag.Offset + {0});", property.Index);
                Indent(3); Out.WriteLine("{0} = value;", property.NativeName);
                Indent(2); Out.WriteLine("}");
                Indent(1); Out.WriteLine("}");
            }
        }

        private void FormatMethods(CellDef def)
        {
            FormatStaticConstructor(def);
            Out.WriteLine();
            FormatConstructor(def);
            Out.WriteLine();
            FormatEqualsTo(def);
            Out.WriteLine();
            FormatGetHashCode(def);
            Out.WriteLine();
            FormatGetType(def);
            Out.WriteLine();
            FormatIsEquivalent(def);
            Out.WriteLine();
            FormatLoad(def);
            if (def.IsEvent)
            {
                Out.WriteLine();
                FormatSerialize(def);
            }
            Out.WriteLine();
            FormatDump(def);
            Out.WriteLine();
            FormatDescribe(def);
            Out.WriteLine();
            FormatInitializer(def);
        }

        private void FormatStaticConstructor(CellDef def)
        {
            string baseTag = def.Base;
            if (String.IsNullOrEmpty(baseTag))
            {
                baseTag = (def.IsEvent ? "Event.tag" : "null");
            }
            else
            {
                baseTag += ".tag";
            }
            Indent(1); Out.WriteLine("static {0}()", def.Name);
            Indent(1); Out.WriteLine("{");
            Indent(2);
            Out.Write("tag = new Tag({0}, typeof({1}), {2}", baseTag, def.Name,
                def.Properties.Count);
            if (def.IsEvent)
            {
                int i;
                string s = ((EventDef)def).Id;
                Out.WriteLine(",");
                Out.Write("                    ");
                if (!Int32.TryParse(s, out i))
                {
                    Out.Write("(int)");
                }
                Out.Write("{0}", s);
            }
            Out.WriteLine(");");
            Indent(1); Out.WriteLine("}");

            if (def.IsEvent)
            {
                Out.WriteLine();
                Indent(1); Out.WriteLine("new public static {0} New()", def.Name);
                Indent(1); Out.WriteLine("{");
                Indent(2); Out.WriteLine("return new {0}();", def.Name);
                Indent(1); Out.WriteLine("}");
            }
        }

        private void FormatConstructor(CellDef def)
        {
            Indent(1); Out.WriteLine("public {0}()", def.Name);
            Indent(2); Out.WriteLine(": base(tag.NumProps)");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("Initialize();");
            Indent(1); Out.WriteLine("}");
            Out.WriteLine();
            Indent(1); Out.WriteLine("protected {0}(int length)", def.Name);
            Indent(2); Out.WriteLine(": base(length + tag.NumProps)");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("Initialize();");
            Indent(1); Out.WriteLine("}");
        }

        private void FormatEqualsTo(CellDef def)
        {
            Indent(1); Out.WriteLine("public override bool EqualsTo(Cell other)");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("if (!base.EqualsTo(other))");
            Indent(2); Out.WriteLine("{");
            Indent(3); Out.WriteLine("return false;");
            Indent(2); Out.WriteLine("}");
            if (def.HasProperties)
            {
                Indent(2); Out.WriteLine("{0} o = ({0})other;", def.Name);
                foreach (var property in def.Properties)
                {
                    //if (Types.IsPrimitive(property.TypeSpec.Type))
                    //{
                        Indent(2); Out.WriteLine("if ({0} != o.{0})", property.NativeName);
                    //}
                    //else
                    //{
                    //    Indent(2); Out.WriteLine("if (!{0}.Equals(o.{0}))", property.NativeName);
                    //}
                    Indent(2); Out.WriteLine("{");
                    Indent(3); Out.WriteLine("return false;");
                    Indent(2); Out.WriteLine("}");
                }
            }
            Indent(2); Out.WriteLine("return true;");
            Indent(1); Out.WriteLine("}");
        }

        private void FormatGetHashCode(CellDef def)
        {
            Indent(1); Out.WriteLine("public override int GetHashCode(Fingerprint fingerprint)");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("var hash = new Hash(base.GetHashCode(fingerprint));");
            if (def.HasProperties)
            {
                Indent(2); Out.WriteLine("if (fingerprint.Length <= tag.Offset)");
                Indent(2); Out.WriteLine("{");
                Indent(3); Out.WriteLine("return hash.Code;");
                Indent(2); Out.WriteLine("}");

                Indent(2); Out.WriteLine("var touched = new Capo<bool>(fingerprint, tag.Offset);");
                foreach (var property in def.Properties)
                {
                    Indent(2); Out.WriteLine("if (touched[{0}])", property.Index);
                    Indent(2); Out.WriteLine("{");
                    Indent(3); Out.WriteLine("hash.Update({0});", property.NativeName);
                    Indent(2); Out.WriteLine("}");
                }
            }
            Indent(2); Out.WriteLine("return hash.Code;");
            Indent(1); Out.WriteLine("}");
        }

        private void FormatGetType(CellDef def)
        {
            if (def.IsEvent)
            {
                Indent(1); Out.WriteLine("public override int GetTypeId()");
                Indent(1); Out.WriteLine("{");
                Indent(2); Out.WriteLine("return tag.TypeId;");
                Indent(1); Out.WriteLine("}");
                Out.WriteLine();
            }
            Indent(1); Out.WriteLine("public override Cell.Tag GetTypeTag() ");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("return tag;");
            Indent(1); Out.WriteLine("}");
        }

        private void FormatIsEquivalent(CellDef def)
        {
            Indent(1); Out.WriteLine("public override bool IsEquivalent(Cell other)");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("if (!base.IsEquivalent(other))");
            Indent(2); Out.WriteLine("{");
            Indent(3); Out.WriteLine("return false;");
            Indent(2); Out.WriteLine("}");
            if (def.HasProperties)
            {
                Indent(2); Out.WriteLine("{0} o = ({0})other;", def.Name);
                Indent(2); Out.WriteLine("var touched = new Capo<bool>(fingerprint, tag.Offset);");
                foreach (var property in def.Properties)
                {
                    Indent(2); Out.WriteLine("if (touched[{0}])", property.Index);
                    Indent(2); Out.WriteLine("{");
                    if (Types.IsPrimitive(property.TypeSpec.Type))
                    {
                        Indent(3); Out.WriteLine("if ({0} != o.{0})", property.NativeName);
                    }
                    else
                    {
                        Indent(3); Out.WriteLine("if ((object){0} == null || !{0}.IsEquivalent(o.{0}))", property.NativeName);
                    }
                    Indent(3); Out.WriteLine("{");
                    Indent(4); Out.WriteLine("return false;");
                    Indent(3); Out.WriteLine("}");
                    Indent(2); Out.WriteLine("}");
                }
            }
            Indent(2); Out.WriteLine("return true;");
            Indent(1); Out.WriteLine("}");
        }

        private void FormatLoad(CellDef def)
        {
            Indent(1); Out.WriteLine("public override void Load(x2.Buffer buffer)");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("base.Load(buffer);");
            if (def.HasProperties)
            {
                Indent(2); Out.WriteLine("var touched = new Capo<bool>(fingerprint, tag.Offset);");
                foreach (var property in def.Properties)
                {
                    Indent(2); Out.WriteLine("if (touched[{0}])", property.Index);
                    Indent(2); Out.WriteLine("{");
                    if (Types.IsPrimitive(property.TypeSpec.Type))
                    {
                        Indent(3); Out.WriteLine("buffer.Read(out {0});", property.NativeName);
                    }
                    else
                    {
                        Indent(3); Out.WriteLine("if ({0} == null) {{ {0} = new {1}(); }}", property.Name, property.NativeType);
                        Indent(3); Out.WriteLine("{0}.Load(buffer);", property.NativeName);
                    }
                    Indent(2); Out.WriteLine("}");
                }
            }
            Indent(1); Out.WriteLine("}");
        }

        private void FormatSerialize(CellDef def)
        {
            Indent(1); Out.WriteLine("public override void Serialize(x2.Buffer buffer)");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("buffer.WriteUInt29(tag.TypeId);");
            Indent(2); Out.WriteLine("this.Dump(buffer);");
            Indent(1); Out.WriteLine("}");
        }

        private void FormatDump(CellDef def)
        {
            Indent(1); Out.WriteLine("protected override void Dump(x2.Buffer buffer)");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("base.Dump(buffer);");
            if (def.HasProperties)
            {
                Indent(2); Out.WriteLine("var touched = new Capo<bool>(fingerprint, tag.Offset);");
                foreach (var property in def.Properties)
                {
                    Indent(2); Out.WriteLine("if (touched[{0}])", property.Index);
                    Indent(2); Out.WriteLine("{");
                    if (Types.IsPrimitive(property.TypeSpec.Type))
                    {
                        Indent(3); Out.WriteLine("buffer.Write({0});", property.NativeName);
                    }
                    else
                    {
                        Indent(3); Out.WriteLine("{0}.Serialize(buffer);", property.NativeName);
                    }
                    Indent(2); Out.WriteLine("}");
                }
            }
            Indent(1); Out.WriteLine("}");
        }

        private void FormatDescribe(CellDef def)
        {
            Indent(1); Out.WriteLine("protected override void Describe(StringBuilder stringBuilder)");
            Indent(1); Out.WriteLine("{");
            Indent(2); Out.WriteLine("base.Describe(stringBuilder);");
            foreach (var property in def.Properties)
            {
                Indent(2);
                if (property.NativeType == "string")
                {
                    Out.WriteLine("stringBuilder.AppendFormat(\" {0}=\\\"{{0}}\\\"\", {1}.Replace(\"\\\"\", \"\\\\\\\"\"));",
                        property.Name, property.NativeName);
                }
                else
                {
                    Out.WriteLine("stringBuilder.AppendFormat(\" {0}={{0}}\", {1});",
                        property.Name, property.NativeName);
                }
            }
            Indent(1); Out.WriteLine("}");
        }

        private void FormatInitializer(CellDef def)
        {
            Indent(1); Out.WriteLine("private void Initialize()");
            Indent(1); Out.WriteLine("{");
            foreach (var property in def.Properties)
            {
                Indent(2);
                Out.WriteLine("{0} = {1};", property.NativeName, property.DefaultValue);
            }
            Indent(1); Out.WriteLine("}");
        }

        private static void PreprocessConsts(ConstsDef def)
        {
            if (def.Type == "string")
            {
                foreach (var constant in def.Constants)
                {
                    constant.Value = "\"" + constant.Value + "\"";
                }
            }
        }

        private static void PreprocessProperties(CellDef def)
        {
            int index = 0;
            foreach (var property in def.Properties)
            {
                property.Index = index++;

                property.NativeName = FirstToLower(property.Name) + "_";
                property.Name = FirstToUpper(property.Name);

                if (Types.IsPrimitive(property.TypeSpec.Type))
                {
                    if (String.IsNullOrEmpty(property.DefaultValue))
                    {
                        property.DefaultValue = defaultValues[property.TypeSpec.Type];
                    }
                    if (property.TypeSpec.Type == "string")
                    {
                        property.DefaultValue = "\"" + property.DefaultValue + "\"";
                    }
                }
                else
                {
                    property.DefaultValue = "null";
                }

                property.NativeType = FormatTypeSpec(property.TypeSpec);
            }
        }

        private static string FormatTypeSpec(TypeSpec typeSpec)
        {
            string type = typeSpec.Type;
            if (!Types.IsBuiltin(type))
            {
                return type;  // custom type
            }
            return Types.IsPrimitive(type) ? nativeTypes[type]
                                           : FormatCollectionType(typeSpec);
        }

        private static string FormatCollectionType(TypeSpec typeSpec)
        {
            var sb = new StringBuilder(nativeTypes[typeSpec.Type]);
            if ((object)typeSpec.Details != null)
            {
                sb.Append('<');
                var leading = true;
                foreach (var detail in typeSpec.Details)
                {
                    if (leading)
                    {
                        leading = false;
                    }
                    else
                    {
                        sb.Append(", ");
                    }
                    sb.Append(FormatTypeSpec(detail));
                }
                sb.Append('>');
            }
            return sb.ToString();
        }

        #region Indentation

        public void Indent()
        {
            ++baseIndentation;
        }

        public void Unindent()
        {
            --baseIndentation;
        }

        private void Indent(int level)
        {
            for (int i = 0; i < (baseIndentation + level); ++i)
            {
                Out.Write(Tab);
            }
        }

        #endregion

        private static string FirstToLower(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                var chars = s.ToCharArray();
                if (Char.IsUpper(chars[0]))
                {
                    chars[0] = Char.ToLower(chars[0]);
                    return new string(chars);
                }
            }
            return s;
        }

        private static string FirstToUpper(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                var chars = s.ToCharArray();
                if (Char.IsLower(chars[0]))
                {
                    chars[0] = Char.ToUpper(chars[0]);
                    return new string(chars);
                }
            }
            return s;
        }
    }
}
