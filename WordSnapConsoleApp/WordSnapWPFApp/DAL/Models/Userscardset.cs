using System;
using System.Collections.Generic;

namespace WordSnapWPFApp.DAL.Models;

public partial class Userscardset
{
    public int Id { get; set; }
    public int UserRef { get; set; }
    public int CardsetRef { get; set; }
    public virtual Cardset CardsetRefNavigation { get; set; } = null!;
    public virtual User UserRefNavigation { get; set; } = null!;
}
