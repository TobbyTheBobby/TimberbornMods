using TimberApi.UIBuilderSystem;
using TimberApi.UIBuilderSystem.CustomElements;
using TimberApi.UIPresets.Buttons;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unstuckify.UIPresets
{
    public class UnstuckifyButton : UnstuckifyButton<UnstuckifyButton>
    {
        protected override UnstuckifyButton BuilderInstance => this;
    }

    public abstract class UnstuckifyButton<TBuilder> : BaseBuilder<TBuilder, LocalizableButton> where TBuilder : BaseBuilder<TBuilder, LocalizableButton>
    {
        private const string LocKey = "Tobbert.Unstuckify.UnstuckifyButton";
        private const string Name = "button";

        private GameButton _localizableButtonBuilder = null!;

        protected override LocalizableButton InitializeRoot()
        {
            _localizableButtonBuilder = UIBuilder.Create<GameButton>();
            _localizableButtonBuilder.SetWidth(new Length(80, LengthUnit.Percent));
            _localizableButtonBuilder.SetHeight(new Length(100, LengthUnit.Percent));
            _localizableButtonBuilder.SetColor(new StyleColor(Color.white));
            _localizableButtonBuilder.SetLocKey(LocKey);
            _localizableButtonBuilder.SetName(Name);
            return _localizableButtonBuilder.Build();
        }
    }
}