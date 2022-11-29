namespace MailTask.Shared
{
    public static class Routes
    {
        public const string Root = "/api";
        public static class V1
        {
            private const string version = "/v1";
            public const string Messages = Root + version + "/messages";
            public const string Inbox = Messages + "/inbox";
            public const string Sent = Messages + "/sent";
            public const string Users = Root + version + "/users";
            /// <summary>
            /// Route has query parameter: ?username={username}
            /// </summary>
            public const string UserSearch = Users + "/search";
            public const string CurrentUser = Users + "/me";
            public const string Login = Users + "/login";
            public const string Logout = Users + "/logout";
        }
    }
}
