namespace DanmakU.Fireables {

    public abstract class Fireable : IFireable {

        public IFireable Child { get; set; }

        public abstract void Fire(DanmakuInitialState state);

        protected void Subfire(DanmakuInitialState state) {
            if (Child == null)
                return;
            Child.Fire(state);
        }

    }

}