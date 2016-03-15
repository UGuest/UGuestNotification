namespace ILuffy.UGuest.Repository
{
    using System;

    class LoginRepository : RepositoryBase, ILoginRepository
    {
        public bool Login()
        {
            return Authenticate(false);
        }
    }
}
