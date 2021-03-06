﻿// auto-generated by xpiler

using System;
using System.Collections.Generic;
using System.Text;

using x2;

namespace x2
{
    public static class SocketLinkEventType
    {
        public const int LinkSessionRecovered = -20;
        public const int KeepaliveEvent = -21;
        public const int KeepaliveTick = -22;
        public const int SessionReq = -23;
        public const int SessionResp = -24;
        public const int SessionAck = -25;

        private static ConstsInfo<int> info;

        static SocketLinkEventType()
        {
            info = new ConstsInfo<int>();
            info.Add("LinkSessionRecovered", -20);
            info.Add("KeepaliveEvent", -21);
            info.Add("KeepaliveTick", -22);
            info.Add("SessionReq", -23);
            info.Add("SessionResp", -24);
            info.Add("SessionAck", -25);
        }

        public static string GetName(int value)
        {
            return info.GetName(value);
        }

        public static int Parse(string name)
        {
            return info.Parse(name);
        }

        public static bool TryParse(string name, out int result)
        {
            return info.TryParse(name, out result);
        }

    }

    public class LinkSessionRecovered : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        private string linkName_;
        private int handle_;
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

        public int Handle
        {
            get { return handle_; }
            set
            {
                fingerprint.Touch(tag.Offset + 1);
                handle_ = value;
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
                    (int)SocketLinkEventType.LinkSessionRecovered);
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
            if (handle_ != o.handle_)
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
                hash.Update(handle_);
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
                if (handle_ != o.handle_)
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
            stringBuilder.AppendFormat(" Handle={0}", handle_);
            stringBuilder.AppendFormat(" Context={0}", context_);
        }
    }

    public class KeepaliveEvent : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        static KeepaliveEvent()
        {
            tag = new Tag(Event.tag, typeof(KeepaliveEvent), 0,
                    (int)SocketLinkEventType.KeepaliveEvent);
        }

        new public static KeepaliveEvent New()
        {
            return new KeepaliveEvent();
        }

        public KeepaliveEvent()
            : base(tag.NumProps)
        {
            Initialize();
        }

        protected KeepaliveEvent(int length)
            : base(length + tag.NumProps)
        {
            Initialize();
        }

        public override bool EqualsTo(Cell other)
        {
            if (!base.EqualsTo(other))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode(Fingerprint fingerprint)
        {
            var hash = new Hash(base.GetHashCode(fingerprint));
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
            return true;
        }

        public override void Deserialize(Deserializer deserializer)
        {
            base.Deserialize(deserializer);
        }

        public override void Deserialize(VerboseDeserializer deserializer)
        {
            base.Deserialize(deserializer);
        }

        public override void Serialize(Serializer serializer)
        {
            base.Serialize(serializer);
        }

        public override void Serialize(VerboseSerializer serializer)
        {
            base.Serialize(serializer);
        }

        public override int GetEncodedLength()
        {
            int length = base.GetEncodedLength();
            return length;
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
        }

        private void Initialize()
        {
        }
    }

    public class KeepaliveTick : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        static KeepaliveTick()
        {
            tag = new Tag(Event.tag, typeof(KeepaliveTick), 0,
                    (int)SocketLinkEventType.KeepaliveTick);
        }

        new public static KeepaliveTick New()
        {
            return new KeepaliveTick();
        }

        public KeepaliveTick()
            : base(tag.NumProps)
        {
        }

        protected KeepaliveTick(int length)
            : base(length + tag.NumProps)
        {
        }

        public override bool EqualsTo(Cell other)
        {
            if (!base.EqualsTo(other))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode(Fingerprint fingerprint)
        {
            var hash = new Hash(base.GetHashCode(fingerprint));
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
            return true;
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
        }
    }

    public class SessionReq : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        private string value_;

        public string Value
        {
            get { return value_; }
            set
            {
                fingerprint.Touch(tag.Offset + 0);
                value_ = value;
            }
        }

        static SessionReq()
        {
            tag = new Tag(Event.tag, typeof(SessionReq), 1,
                    (int)SocketLinkEventType.SessionReq);
        }

        new public static SessionReq New()
        {
            return new SessionReq();
        }

        public SessionReq()
            : base(tag.NumProps)
        {
            Initialize();
        }

        protected SessionReq(int length)
            : base(length + tag.NumProps)
        {
            Initialize();
        }

        public override bool EqualsTo(Cell other)
        {
            if (!base.EqualsTo(other))
            {
                return false;
            }
            SessionReq o = (SessionReq)other;
            if (value_ != o.value_)
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
                hash.Update(value_);
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
            SessionReq o = (SessionReq)other;
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                if (value_ != o.value_)
                {
                    return false;
                }
            }
            return true;
        }

        public override void Deserialize(Deserializer deserializer)
        {
            base.Deserialize(deserializer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                deserializer.Read(out value_);
            }
        }

        public override void Deserialize(VerboseDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            deserializer.Read("Value", out value_);
        }

        public override void Serialize(Serializer serializer)
        {
            base.Serialize(serializer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                serializer.Write(value_);
            }
        }

        public override void Serialize(VerboseSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write("Value", value_);
        }

        public override int GetEncodedLength()
        {
            int length = base.GetEncodedLength();
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                length += Serializer.GetEncodedLength(value_);
            }
            return length;
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
            stringBuilder.AppendFormat(" Value=\"{0}\"", value_.Replace("\"", "\\\""));
        }

        private void Initialize()
        {
            value_ = "";
        }
    }

    public class SessionResp : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        private string value_;

        public string Value
        {
            get { return value_; }
            set
            {
                fingerprint.Touch(tag.Offset + 0);
                value_ = value;
            }
        }

        static SessionResp()
        {
            tag = new Tag(Event.tag, typeof(SessionResp), 1,
                    (int)SocketLinkEventType.SessionResp);
        }

        new public static SessionResp New()
        {
            return new SessionResp();
        }

        public SessionResp()
            : base(tag.NumProps)
        {
            Initialize();
        }

        protected SessionResp(int length)
            : base(length + tag.NumProps)
        {
            Initialize();
        }

        public override bool EqualsTo(Cell other)
        {
            if (!base.EqualsTo(other))
            {
                return false;
            }
            SessionResp o = (SessionResp)other;
            if (value_ != o.value_)
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
                hash.Update(value_);
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
            SessionResp o = (SessionResp)other;
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                if (value_ != o.value_)
                {
                    return false;
                }
            }
            return true;
        }

        public override void Deserialize(Deserializer deserializer)
        {
            base.Deserialize(deserializer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                deserializer.Read(out value_);
            }
        }

        public override void Deserialize(VerboseDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            deserializer.Read("Value", out value_);
        }

        public override void Serialize(Serializer serializer)
        {
            base.Serialize(serializer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                serializer.Write(value_);
            }
        }

        public override void Serialize(VerboseSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write("Value", value_);
        }

        public override int GetEncodedLength()
        {
            int length = base.GetEncodedLength();
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                length += Serializer.GetEncodedLength(value_);
            }
            return length;
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
            stringBuilder.AppendFormat(" Value=\"{0}\"", value_.Replace("\"", "\\\""));
        }

        private void Initialize()
        {
            value_ = "";
        }
    }

    public class SessionAck : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        static SessionAck()
        {
            tag = new Tag(Event.tag, typeof(SessionAck), 0,
                    (int)SocketLinkEventType.SessionAck);
        }

        new public static SessionAck New()
        {
            return new SessionAck();
        }

        public SessionAck()
            : base(tag.NumProps)
        {
            Initialize();
        }

        protected SessionAck(int length)
            : base(length + tag.NumProps)
        {
            Initialize();
        }

        public override bool EqualsTo(Cell other)
        {
            if (!base.EqualsTo(other))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode(Fingerprint fingerprint)
        {
            var hash = new Hash(base.GetHashCode(fingerprint));
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
            return true;
        }

        public override void Deserialize(Deserializer deserializer)
        {
            base.Deserialize(deserializer);
        }

        public override void Deserialize(VerboseDeserializer deserializer)
        {
            base.Deserialize(deserializer);
        }

        public override void Serialize(Serializer serializer)
        {
            base.Serialize(serializer);
        }

        public override void Serialize(VerboseSerializer serializer)
        {
            base.Serialize(serializer);
        }

        public override int GetEncodedLength()
        {
            int length = base.GetEncodedLength();
            return length;
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
        }

        private void Initialize()
        {
        }
    }
}
