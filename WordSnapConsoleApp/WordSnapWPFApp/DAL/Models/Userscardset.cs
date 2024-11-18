// <copyright file="Userscardset.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.DAL.Models;

/// <summary>
/// userscardset.
/// </summary>
public partial class Userscardset
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
    /// Gets or sets cardset reference.
    /// </summary>
    public int CardsetRef { get; set; }

    /// <summary>
    /// Gets or sets cardset reference navigation.
    /// </summary>
    public virtual Cardset CardsetRefNavigation { get; set; } = null!;

    /// <summary>
    /// Gets or sets user reference navigation.
    /// </summary>
    public virtual User UserRefNavigation { get; set; } = null!;
}
