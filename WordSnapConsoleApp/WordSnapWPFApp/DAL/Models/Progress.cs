// <copyright file="Progress.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.DAL.Models;

/// <summary>
/// progress.
/// </summary>
public partial class Progress
{
    /// <summary>
    /// Gets or sets user reference.
    /// </summary>
    public int UserRef { get; set; }

    /// <summary>
    /// Gets or sets cardset reference.
    /// </summary>
    public int CardsetRef { get; set; }

    /// <summary>
    /// Gets or sets the last accessed time.
    /// </summary>
    public DateTime? LastAccessed { get; set; }

    /// <summary>
    /// Gets or sets success rate.
    /// </summary>
    public double? SuccessRate { get; set; }

    /// <summary>
    /// Gets or sets cardset reference navigation.
    /// </summary>
    public virtual Cardset CardsetRefNavigation { get; set; } = null!;

    /// <summary>
    /// Gets or sets user reference navigation.
    /// </summary>
    public virtual User UserRefNavigation { get; set; } = null!;
}
