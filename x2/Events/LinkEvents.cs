﻿// auto-generated by xpiler

using System;
using System.Collections.Generic;
using System.Text;

using x2;

namespace x2.Events
{
    public class LinkSessionConnected : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        private string linkName_;
        private bool result_;
        private object context_;

        public string LinkName
        {
            get { return linkName_; }
            set
            {
                fingerprint.Touch(tag.Offset + 0);
                linkName_ = value;
            }
        }

        public bool Result
        {
            get { return result_; }
            set
            {
                fingerprint.Touch(tag.Offset + 1);
                result_ = value;
            }
        }

        public object Context
        {
            get { return context_; }
            set
            {
                fingerprint.Touch(tag.Offset + 2);
                context_ = value;
            }
        }

        static LinkSessionConnected()
        {
            tag = new Tag(Event.tag, typeof(LinkSessionConnected), 3,
                    (int)BuiltInType.LinkSessionConnected);
        }

        new public static LinkSessionConnected New()
        {
            return new LinkSessionConnected();
        }

        public LinkSessionConnected()
            : base(tag.NumProps)
        {
        }

        protected LinkSessionConnected(int length)
            : base(length + tag.NumProps)
        {
        }

        public override bool EqualsTo(Cell other)
        {
            if (!base.EqualsTo(other))
            {
                return false;
            }
            LinkSessionConnected o = (LinkSessionConnected)other;
            if (linkName_ != o.linkName_)
            {
                return false;
            }
            if (result_ != o.result_)
            {
                return false;
            }
            if (context_ != o.context_)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode(Fingerprint fingerprint)
        {
            var hash = new Hash(base.GetHashCode(fingerprint));
            if (fingerprint.Length <= tag.Offset)
            {
                return hash.Code;
            }
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                hash.Update(linkName_);
            }
            if (touched[1])
            {
                hash.Update(result_);
            }
            if (touched[2])
            {
                hash.Update(context_);
            }
            return hash.Code;
        }

        public override int GetTypeId()
        {
            return tag.TypeId;
        }

        public override Cell.Tag GetTypeTag() 
        {
            return tag;
        }

        public override bool IsEquivalent(Cell other)
        {
            if (!base.IsEquivalent(other))
            {
                return false;
            }
            LinkSessionConnected o = (LinkSessionConnected)other;
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                if (linkName_ != o.linkName_)
                {
                    return false;
                }
            }
            if (touched[1])
            {
                if (result_ != o.result_)
                {
                    return false;
                }
            }
            if (touched[2])
            {
                if (context_ != o.context_)
                {
                    return false;
                }
            }
            return true;
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
            stringBuilder.AppendFormat(" LinkName=\"{0}\"", linkName_.Replace("\"", "\\\""));
            stringBuilder.AppendFormat(" Result={0}", result_);
            stringBuilder.AppendFormat(" Context={0}", context_);
        }
    }

    public class LinkSessionDisconnected : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        private string linkName_;
        private bool result_;
        private object context_;

        public string LinkName
        {
            get { return linkName_; }
            set
            {
                fingerprint.Touch(tag.Offset + 0);
                linkName_ = value;
            }
        }

        public bool Result
        {
            get { return result_; }
            set
            {
                fingerprint.Touch(tag.Offset + 1);
                result_ = value;
            }
        }

        public object Context
        {
            get { return context_; }
            set
            {
                fingerprint.Touch(tag.Offset + 2);
                context_ = value;
            }
        }

        static LinkSessionDisconnected()
        {
            tag = new Tag(Event.tag, typeof(LinkSessionDisconnected), 3,
                    (int)BuiltInType.LinkSessionDisconnected);
        }

        new public static LinkSessionDisconnected New()
        {
            return new LinkSessionDisconnected();
        }

        public LinkSessionDisconnected()
            : base(tag.NumProps)
        {
        }

        protected LinkSessionDisconnected(int length)
            : base(length + tag.NumProps)
        {
        }

        public override bool EqualsTo(Cell other)
        {
            if (!base.EqualsTo(other))
            {
                return false;
            }
            LinkSessionDisconnected o = (LinkSessionDisconnected)other;
            if (linkName_ != o.linkName_)
            {
                return false;
            }
            if (result_ != o.result_)
            {
                return false;
            }
            if (context_ != o.context_)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode(Fingerprint fingerprint)
        {
            var hash = new Hash(base.GetHashCode(fingerprint));
            if (fingerprint.Length <= tag.Offset)
            {
                return hash.Code;
            }
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                hash.Update(linkName_);
            }
            if (touched[1])
            {
                hash.Update(result_);
            }
            if (touched[2])
            {
                hash.Update(context_);
            }
            return hash.Code;
        }

        public override int GetTypeId()
        {
            return tag.TypeId;
        }

        public override Cell.Tag GetTypeTag() 
        {
            return tag;
        }

        public override bool IsEquivalent(Cell other)
        {
            if (!base.IsEquivalent(other))
            {
                return false;
            }
            LinkSessionDisconnected o = (LinkSessionDisconnected)other;
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                if (linkName_ != o.linkName_)
                {
                    return false;
                }
            }
            if (touched[1])
            {
                if (result_ != o.result_)
                {
                    return false;
                }
            }
            if (touched[2])
            {
                if (context_ != o.context_)
                {
                    return false;
                }
            }
            return true;
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
            stringBuilder.AppendFormat(" LinkName=\"{0}\"", linkName_.Replace("\"", "\\\""));
            stringBuilder.AppendFormat(" Result={0}", result_);
            stringBuilder.AppendFormat(" Context={0}", context_);
        }
    }

    public class LinkSessionRecovered : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        private string linkName_;
        private int oldHandle_;
        private object context_;

        public string LinkName
        {
            get { return linkName_; }
            set
            {
                fingerprint.Touch(tag.Offset + 0);
                linkName_ = value;
            }
        }

        public int OldHandle
        {
            get { return oldHandle_; }
            set
            {
                fingerprint.Touch(tag.Offset + 1);
                oldHandle_ = value;
            }
        }

        public object Context
        {
            get { return context_; }
            set
            {
                fingerprint.Touch(tag.Offset + 2);
                context_ = value;
            }
        }

        static LinkSessionRecovered()
        {
            tag = new Tag(Event.tag, typeof(LinkSessionRecovered), 3,
                    (int)BuiltInType.LinkSessionRecovered);
        }

        new public static LinkSessionRecovered New()
        {
            return new LinkSessionRecovered();
        }

        public LinkSessionRecovered()
            : base(tag.NumProps)
        {
        }

        protected LinkSessionRecovered(int length)
            : base(length + tag.NumProps)
        {
        }

        public override bool EqualsTo(Cell other)
        {
            if (!base.EqualsTo(other))
            {
                return false;
            }
            LinkSessionRecovered o = (LinkSessionRecovered)other;
            if (linkName_ != o.linkName_)
            {
                return false;
            }
            if (oldHandle_ != o.oldHandle_)
            {
                return false;
            }
            if (context_ != o.context_)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode(Fingerprint fingerprint)
        {
            var hash = new Hash(base.GetHashCode(fingerprint));
            if (fingerprint.Length <= tag.Offset)
            {
                return hash.Code;
            }
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                hash.Update(linkName_);
            }
            if (touched[1])
            {
                hash.Update(oldHandle_);
            }
            if (touched[2])
            {
                hash.Update(context_);
            }
            return hash.Code;
        }

        public override int GetTypeId()
        {
            return tag.TypeId;
        }

        public override Cell.Tag GetTypeTag() 
        {
            return tag;
        }

        public override bool IsEquivalent(Cell other)
        {
            if (!base.IsEquivalent(other))
            {
                return false;
            }
            LinkSessionRecovered o = (LinkSessionRecovered)other;
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                if (linkName_ != o.linkName_)
                {
                    return false;
                }
            }
            if (touched[1])
            {
                if (oldHandle_ != o.oldHandle_)
                {
                    return false;
                }
            }
            if (touched[2])
            {
                if (context_ != o.context_)
                {
                    return false;
                }
            }
            return true;
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
            stringBuilder.AppendFormat(" LinkName=\"{0}\"", linkName_.Replace("\"", "\\\""));
            stringBuilder.AppendFormat(" OldHandle={0}", oldHandle_);
            stringBuilder.AppendFormat(" Context={0}", context_);
        }
    }
}
