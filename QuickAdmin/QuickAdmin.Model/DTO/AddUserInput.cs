namespace QuickAdmin.Model.DTO
{
    public class AddUserInput
    {
        /// <summary>
        /// 用户Id,修改用
        /// </summary>
        public string Id { get; set; }    
        
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }
    }
}
