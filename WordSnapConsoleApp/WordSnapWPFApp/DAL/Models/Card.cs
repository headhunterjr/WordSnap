using System;
using System.Collections.Generic;

namespace WordSnapWPFApp.DAL.Models;

public partial class Card
{
    public int Id { get; set; }
    public int CardsetRef { get; set; }
    public string WordEn { get; set; } = null!;
    public string WordUa { get; set; } = null!;
    public string? Comment { get; set; }
    public virtual Cardset CardsetRefNavigation { get; set; } = null!;
}
