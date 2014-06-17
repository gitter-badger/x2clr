// auto-generated by xpiler

using System;
using System.Text;

using x2;

namespace x2.Links.SocketLink
{
    public static class SocketLinkEventType
    {
        public const int KeepaliveEvent = -10;
        public const int KeepaliveTick = -11;
        public const int HandshakeReq = -20;
        public const int HandshakeResp = -20;
        public const int HandshakeAck = -20;
        public const int SessionKeyReq = -30;
        public const int SessionKeyResp = -31;
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

        public override void Load(x2.Buffer buffer)
        {
            base.Load(buffer);
        }

        public override void Serialize(x2.Buffer buffer)
        {
            buffer.Write(tag.TypeId);
            this.Dump(buffer);
        }

        protected override void Dump(x2.Buffer buffer)
        {
            base.Dump(buffer);
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

        private string linkName_;

        public string LinkName
        {
            get { return linkName_; }
            set
            {
                fingerprint.Touch(tag.Offset + 0);
                linkName_ = value;
            }
        }

        static KeepaliveTick()
        {
            tag = new Tag(Event.tag, typeof(KeepaliveTick), 1,
                    (int)SocketLinkEventType.KeepaliveTick);
        }

        new public static KeepaliveTick New()
        {
            return new KeepaliveTick();
        }

        public KeepaliveTick()
            : base(tag.NumProps)
        {
            Initialize();
        }

        protected KeepaliveTick(int length)
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
            KeepaliveTick o = (KeepaliveTick)other;
            if (linkName_ != o.linkName_)
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
            KeepaliveTick o = (KeepaliveTick)other;
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                if (linkName_ != o.linkName_)
                {
                    return false;
                }
            }
            return true;
        }

        public override void Load(x2.Buffer buffer)
        {
            base.Load(buffer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                buffer.Read(out linkName_);
            }
        }

        public override void Serialize(x2.Buffer buffer)
        {
            buffer.Write(tag.TypeId);
            this.Dump(buffer);
        }

        protected override void Dump(x2.Buffer buffer)
        {
            base.Dump(buffer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                buffer.Write(linkName_);
            }
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
            stringBuilder.AppendFormat(" LinkName=\"{0}\"", linkName_.Replace("\"", "\\\""));
        }

        private void Initialize()
        {
            linkName_ = "";
        }
    }

    public class HandshakeReq : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        private byte[] data_;

        public byte[] Data
        {
            get { return data_; }
            set
            {
                fingerprint.Touch(tag.Offset + 0);
                data_ = value;
            }
        }

        static HandshakeReq()
        {
            tag = new Tag(Event.tag, typeof(HandshakeReq), 1,
                    (int)SocketLinkEventType.HandshakeReq);
        }

        new public static HandshakeReq New()
        {
            return new HandshakeReq();
        }

        public HandshakeReq()
            : base(tag.NumProps)
        {
            Initialize();
        }

        protected HandshakeReq(int length)
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
            HandshakeReq o = (HandshakeReq)other;
            if (data_ != o.data_)
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
                hash.Update(data_);
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
            HandshakeReq o = (HandshakeReq)other;
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                if (data_ != o.data_)
                {
                    return false;
                }
            }
            return true;
        }

        public override void Load(x2.Buffer buffer)
        {
            base.Load(buffer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                buffer.Read(out data_);
            }
        }

        public override void Serialize(x2.Buffer buffer)
        {
            buffer.Write(tag.TypeId);
            this.Dump(buffer);
        }

        protected override void Dump(x2.Buffer buffer)
        {
            base.Dump(buffer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                buffer.Write(data_);
            }
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
            stringBuilder.AppendFormat(" Data={0}", data_);
        }

        private void Initialize()
        {
            data_ = null;
        }
    }

    public class HandshakeResp : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        private byte[] data_;

        public byte[] Data
        {
            get { return data_; }
            set
            {
                fingerprint.Touch(tag.Offset + 0);
                data_ = value;
            }
        }

        static HandshakeResp()
        {
            tag = new Tag(Event.tag, typeof(HandshakeResp), 1,
                    (int)SocketLinkEventType.HandshakeResp);
        }

        new public static HandshakeResp New()
        {
            return new HandshakeResp();
        }

        public HandshakeResp()
            : base(tag.NumProps)
        {
            Initialize();
        }

        protected HandshakeResp(int length)
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
            HandshakeResp o = (HandshakeResp)other;
            if (data_ != o.data_)
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
                hash.Update(data_);
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
            HandshakeResp o = (HandshakeResp)other;
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                if (data_ != o.data_)
                {
                    return false;
                }
            }
            return true;
        }

        public override void Load(x2.Buffer buffer)
        {
            base.Load(buffer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                buffer.Read(out data_);
            }
        }

        public override void Serialize(x2.Buffer buffer)
        {
            buffer.Write(tag.TypeId);
            this.Dump(buffer);
        }

        protected override void Dump(x2.Buffer buffer)
        {
            base.Dump(buffer);
            var touched = new Capo<bool>(fingerprint, tag.Offset);
            if (touched[0])
            {
                buffer.Write(data_);
            }
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            base.Describe(stringBuilder);
            stringBuilder.AppendFormat(" Data={0}", data_);
        }

        private void Initialize()
        {
            data_ = null;
        }
    }

    public class HandshakeAck : Event
    {
        new protected static readonly Tag tag;

        new public static int TypeId { get { return tag.TypeId; } }

        static HandshakeAck()
        {
            tag = new Tag(Event.tag, typeof(HandshakeAck), 0,
                    (int)SocketLinkEventType.HandshakeAck);
        }

        new public static HandshakeAck New()
        {
            return new HandshakeAck();
        }

        public HandshakeAck()
            : base(tag.NumProps)
        {
            Initialize();
        }

        protected HandshakeAck(int length)
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

        public override void Load(x2.Buffer buffer)
        {
            base.Load(buffer);
        }

        public override void Serialize(x2.Buffer buffer)
        {
            buffer.Write(tag.TypeId);
            this.Dump(buffer);
        }

        protected override void Dump(x2.Buffer buffer)
        {
            base.Dump(buffer);
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
