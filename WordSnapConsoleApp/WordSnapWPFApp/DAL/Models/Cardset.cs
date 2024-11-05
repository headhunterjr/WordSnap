using System;
using System.Collections.Generic;

namespace WordSnapWPFApp.DAL.Models;

public partial class Cardset
{
    public int Id { get; set; }
    public int UserRef { get; set; }
    public string Name { get; set; } = null!;
    public bool? IsPublic { get; set; }
    public DateTime? CreatedAt { get; set; }
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();
    public virtual User UserRefNavigation { get; set; } = null!;
    public virtual ICollection<Userscardset> Userscardsets { get; set; } = new List<Userscardset>();
}
