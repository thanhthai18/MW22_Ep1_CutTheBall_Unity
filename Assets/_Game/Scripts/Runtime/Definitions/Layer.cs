namespace Runtime.Definition
{
    public class Layer
    {
        #region Members

        public const int BALL_LAYER = 7;
        public const int BOOM_LAYER = 9;
        public const int SPRITE_RENDERER_BUTTON_LAYER = 13;
        public const int SPLIT_LAYER = 16;
        public static int BALL_LAYER_MASK = 1 << BALL_LAYER;
        public static int BOOM_LAYER_MASK = 1 << BOOM_LAYER;
        public static int SPRITE_RENDERER_BUTTON_LAYER_MASK = 1 << SPRITE_RENDERER_BUTTON_LAYER;
        public static int SPLIT_LAYER_LAYER_MASK = 1 << SPLIT_LAYER;

        #endregion Members
    }
}