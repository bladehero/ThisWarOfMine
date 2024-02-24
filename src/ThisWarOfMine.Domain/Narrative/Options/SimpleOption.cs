namespace ThisWarOfMine.Domain.Narrative.Options
{
    internal sealed class SimpleOption : Option
    {
        public override string Text { get; }
        public override bool IsRedirecting => false;

        private SimpleOption(OptionGroup group, Guid guid, string text)
            : base(group, guid) => Text = text;

        internal static SimpleOption Create(OptionGroup group, Guid guid, string text) => new(group, guid, text);
    }
}
