namespace Hotel.Entities
{
    public class RolesIdentifiers
    {
        const string Admin = "Admin";
        const string Worker = "Worker";
        const string WorkerRead = "WorkerRead";
        const string User = "User";


        public static bool IsAdmin(Role role)
        {
            return role.Identifier == Admin;
        }

        public static bool IsWorker(Role role)
        {
            return role.Identifier == Worker;
        }

        public static bool IsUser(Role role)
        {
            return role.Identifier == User;
        }

        public static bool IsReadOnly(string identifier)
        {
            return identifier == WorkerRead;
        }
    }
}