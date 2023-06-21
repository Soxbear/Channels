namespace Soxbear.Channels {
    public class Channel {
        private object v;
        public object value {
            get { return v; }
        }

        public delegate void updateNotifier(object value);

        public updateNotifier onUpdate;

        public void setValue(object value) {
            v = value;
            onUpdate(value);
        }
    }
}