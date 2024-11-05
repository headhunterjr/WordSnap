using System;
using System.Collections.Generic;

namespace WordSnapWPFApp.DAL.Models;

public partial class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public bool? IsVerified { get; set; }
    public DateTime? CreatedAt { get; set; }
    public virtual ICollection<Cardset> Cardsets { get; set; } = new List<Cardset>();
    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();
    public virtual ICollection<Userscardset> Userscardsets { get; set; } = new List<Userscardset>();
}
