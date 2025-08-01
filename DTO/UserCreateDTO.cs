namespace RODXUsers.DTO
{
    public class UserCreateDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public int FkIdRol { get; set; }
        public string Password { get; set; }
    }
}
