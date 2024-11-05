using System;
using System.Collections.Generic;

namespace WordSnapWPFApp.DAL.Models;

public partial class Progress
{
    public int UserRef { get; set; }
    public int CardsetRef { get; set; }
    public DateTime? LastAccessed { get; set; }
    public double? SuccessRate { get; set; }
    public virtual Cardset CardsetRefNavigation { get; set; } = null!;
    public virtual User UserRefNavigation { get; set; } = null!;
}
