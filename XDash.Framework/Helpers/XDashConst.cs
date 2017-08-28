namespace XDash.Framework.Helpers
{
    public static class XDashConst
    {
        // Default general and connectivity constants
        // PLEASE DO NOT CHANGE ANYTHING IN HERE IF YOU DON'T KNOW EXACTLY WHAT YOU ARE DOING !

        public const int MINIMUM_PORT_VALUE = 1024;

        /// <summary>
        /// Default length used for defining string buffers in the framework . Used in processes such as
        /// getting the remote device's name .
        /// </summary>
        public const int DEFAULT_STRING_BUFFER_LENGTH = 256;

        /// <summary>
        /// Default length used for defining int buffers in the framework . Used in processes such as
        /// getting the files count or the file size .
        /// </summary>
        public const int DEFAULT_INT_BUFFER_LENGTH = sizeof(int);

        public const int DEFAULT_LONG_BUFFER_LENGTH = sizeof(long);

        /// <summary>
        /// Default size of a DashNode's data buffer / Default TCP transfer packet size .
        /// </summary>
        public const int DEFAULT_TRANSFER_BUFFER_SIZE = 1024;

        /// <summary>
        /// Default port on which the XDashBeacon is listening and the XDashScanner is querying .
        /// </summary>
        public const int DEFAULT_BEACON_SCAN_PORT = 5401;

        /// <summary>
        /// Default port on which the XDashBeacon will send the response back to the XDashScanner .
        /// </summary>
        public const int DEFAULT_SCAN_RESPONSE_PORT = 5402;

        /// <summary>
        /// Default port on which XDashSenders are transferring data .
        /// </summary>
        public const int DEFAULT_TRANSFER_PORT = 5403;

        /// <summary>
        /// Default port on which XDashReceivers are feeding back responses to XDashSenders .
        /// </summary>
        public const int DEFAULT_TRANSFER_FEEDBACK_PORT = 5404;

        /// <summary>
        /// Broadcast subnet suffix . Used to get the right subnet of the subnetwork on which a speciffic network adapter
        /// is connected .
        /// </summary>
        public const string BROADCAST_SUBNET_SUFFIX = ".255";

        public const string AUTH_TOKEN = "dash.auth.token";

        /// <summary>
        /// Used to identify and save preferences and settings .
        /// </summary>
        public static class Prefs
        {
            // Shared preferences group name
            public const string GLOBAL_PREFS = "GLOBAL_PREFS";

            // Shared preferences names
            public const string RUN_AS_CLIENT = "RUN_AS_CLIENT";
            public const string AUTO_APPROVE_DASHES = "AUTO_APPROVE_DASHES";
            public const string ASK_WHERE_TO_SAVE = "ASK_WHERE_TO_SAVE";
            public const string PRESET_PATH = "PRESET_PATH";
            public const string OVERWRITE_EXISTING = "OVERWRITE_EXISTING";
            public const string DEBUG_DETECT_OWN_IP = "DEBUG_DETECT_OWN_IP";

            public const string HINT_TAPPED = "HINT_TAPPED";

            public const string DEVICE_NAME = "DEVICE_NAME";
            public const string DEVICE_NAME_DEF_DROID = "DashDroid Device";
            public const string DEVICE_NAME_DEF_WIN = "DashWin Device";

            public const string REMOTE_CLIENT = "REMOTE_CLIENT";

            public const string PIN_VALIDATION_RESULT = "PIN_VALIDATION_RESULT";
            public const string RECEIVED_PIN = "RECEIVED_PIN";

            // Shared preferences values
            public const string NOT_SET = "NOT_SET";
        }

        /// <summary>
        /// // Tags that will appear in the logging system so that you
        /// will know which Dash component has thrown which exception
        /// </summary>
        public static class LogTags
        {
            /// <summary>
            /// Exceptions caused by the DashSender .
            /// </summary>
            public const string DASH_TCPSENDER = "DASH_TCPSENDER";

            /// <summary>
            /// Exceptions caused by the DashReceiver .
            /// </summary>
            public const string DASH_TCPRECEIVER = "DASH_TCPRECEIVER";

            /// <summary>
            /// Wi-Fi Multicast lock tag - used for debugging
            /// </summary>
            public const string WIFI_MULTICAST_LOCK_TAG = "dash.WIFI_MULTICAST_LOCK_TAG";
        }
    }
}
