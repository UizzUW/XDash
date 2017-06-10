namespace XDash.Framework.Helpers
{
    public static class XDashConst
    {
        // Default general and connectivity constants
        // DO NOT CHANGE ANYTHING HERE IF YOU DON'T KNOW EXACTLY WHAT YOU ARE DOING !

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
        /// Default port on which the DashBeacon is broadcasting and the DashBeaconQuery is querying .
        /// </summary>
        public const int DEFAULT_DISCOVERY_PORT = 5401;
        /// <summary>
        /// The interval between each broadcast of a DashBeacon .
        /// </summary>
        public const int DEFAULT_BEACON_INTERVAL = 1000;
        /// <summary>
        /// Default port on which DashNodes are transferring receiving data .
        /// </summary>
        public const int DEFAULT_TRANSFER_PORT = 5403;
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
        /// Shared preferences status
        /// </summary>
        public enum PrefsStatus
        {
            PREFS_LOADING,
            PREFS_STANDBY,
            PREFS_SAVING
        }

        /// <summary>
        /// Possible reasons for the app's termination .
        /// </summary>
        public enum ExitErrorCodes
        {
            EXIT_NORMAL,
            EXIT_ALREADY_OPENED,
            EXIT_UPDATE,
            EXIT_DISABLE_CONTEXT,
            EXIT_NO_ADAPTER_SET,
            EXIT_NO_CONNECTION
        }

        /// <summary>
        /// Used to handle the user input regarding a dash approval
        /// </summary>
        public static class Handler
        {
            // Parameter extra names
            public const string REMOTE_IP = "REMOTE_IP";
            public const string FILENAME = "FILENAME";
            public const string FILESIZE = "FILESIZE";
            public const string FILECOUNT = "FILECOUNT";

            // Result extra name
            public const string HANDLE_RESULT = "HANDLE_RESULT";
        }

        /// <summary>
        /// Used to pass network data among app components .
        /// </summary>
        public static class Discovery
        {
            public const string REMOTE_DEVICE_CLIENT_INFO = "REMOTE_DEVICE_CLIENT_INFO";
            public const string SERIAL_DATA = "SERIAL_DATA";
            public const string IS_BROADCASTING = "IS_BROADCASTING";
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
