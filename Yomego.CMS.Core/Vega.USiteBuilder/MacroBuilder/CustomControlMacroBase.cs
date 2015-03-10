namespace Vega.USiteBuilder.MacroBuilder
{
    public abstract class CustomControlMacroBase : MacroBase
    {
        public abstract string Assembly { get; }
        public abstract string Type { get; }
    }
}
