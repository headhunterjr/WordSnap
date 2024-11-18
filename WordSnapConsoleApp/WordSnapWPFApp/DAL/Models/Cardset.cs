// <copyright file="Cardset.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.DAL.Models;

/// <summary>
/// cardset.
/// </summary>
public partial class Cardset
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets user reference.
    /// </summary>
    public int UserRef { get; set; }

    /// <summary>
    /// Gets or sets name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets privacy setting.
    /// </summary>
    public bool? IsPublic { get; set; }

    /// <summary>
    /// Gets or sets time of creation.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets cards.
    /// </summary>
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    /// <summary>
    /// Gets or sets progresses.
    /// </summary>
    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    /// <summary>
    /// Gets or sets user reference navigation.
    /// </summary>
    public virtual User UserRefNavigation { get; set; } = null!;

    /// <summary>
    /// Gets or sets userscardsets.
    /// </summary>
    public virtual ICollection<Userscardset> Userscardsets { get; set; } = new List<Userscardset>();
}
