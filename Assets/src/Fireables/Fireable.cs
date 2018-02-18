namespace DanmakU.Fireables {

    public abstract class Fireable : IFireable {

        public IFireable Child { get; set; }

        public abstract void Fire(DanamkuConfig state);

        protected void Subfire(DanamkuConfig state) {
            if (Child == null)
                return;
            Child.Fire(state);
        }

    }

}