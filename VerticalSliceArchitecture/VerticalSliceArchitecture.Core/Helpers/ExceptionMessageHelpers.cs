namespace VerticalSliceArchitecture.Core.Helpers
{
    public static class ExceptionMessageHelpers
    {
        public static string NotEmpty(string paramName)
            => $"{paramName} can not be empty.";

        public static string NoLongerThen(string paramName, int maxValue)
            => $"{paramName} can not be longer then {maxValue} characters.";
    }
}
