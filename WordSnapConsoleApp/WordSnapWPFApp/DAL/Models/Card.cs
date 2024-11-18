// <copyright file="Card.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.DAL.Models;

/// <summary>
/// card entity.
/// </summary>
public partial class Card
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets cardset reference.
    /// </summary>
    public int CardsetRef { get; set; }

    /// <summary>
    /// Gets or sets word in english.
    /// </summary>
    public string WordEn { get; set; } = null!;

    /// <summary>
    /// Gets or sets word in ukrainian.
    /// </summary>
    public string WordUa { get; set; } = null!;

    /// <summary>
    /// Gets or sets a comment.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Gets or sets cardset reference navigation.
    /// </summary>
    public virtual Cardset CardsetRefNavigation { get; set; } = null!;
}
