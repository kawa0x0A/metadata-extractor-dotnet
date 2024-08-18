namespace MetadataExtractor.Util
{
    public static class PhotographicConversions
    {
        private const double Ln2 = 0.69314718055994530941723212145818d;
        private const double RootTwo = 1.4142135623730950488016887242097d;

        public static double ApertureToFStop(double aperture) => Math.Pow(RootTwo, aperture);

        public static double ShutterSpeedToExposureTime(double shutterSpeed) => (float)(1 / Math.Exp(shutterSpeed * Ln2));
    }
}
