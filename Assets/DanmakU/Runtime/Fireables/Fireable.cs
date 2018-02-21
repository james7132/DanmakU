namespace DanmakU.Fireables {

    public abstract class Fireable : IFireable {

        public IFireable Child { get; set; }

        public abstract void Fire(DanmakuConfig state);

        protected void Subfire(DanmakuConfig state) {
            if (Child == null)
                return;
            Child.Fire(state);
        }

    }

}