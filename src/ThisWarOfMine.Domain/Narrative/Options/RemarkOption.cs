﻿namespace ThisWarOfMine.Domain.Narrative.Options;

internal sealed class RemarkOption : Option
{
    public RemarkOption(OptionGroup group, string text)
        : base(group) => Text = text;

    public override string Text { get; }
    public override bool IsRedirecting => false;
    public override bool IsSelectable => false;

    internal static RemarkOption Create(OptionGroup group, string text)
    {
        return new RemarkOption(group, text);
    }
}