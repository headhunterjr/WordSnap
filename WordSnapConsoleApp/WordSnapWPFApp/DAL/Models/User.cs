// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.DAL.Models;

/// <summary>
/// user.
/// </summary>
public partial class User
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets username.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets password hash.
    /// </summary>
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// Gets or sets password salt.
    /// </summary>
    public string PasswordSalt { get; set; } = null!;

    /// <summary>
    /// Gets or sets account verification status.
    /// </summary>
    public bool? IsVerified { get; set; }

    /// <summary>
    /// Gets or sets the time the account was created.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets cardsets.
    /// </summary>
    public virtual ICollection<Cardset> Cardsets { get; set; } = new List<Cardset>();

    /// <summary>
    /// Gets or sets progresses.
    /// </summary>
    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    /// <summary>
    /// Gets or sets userscardsets.
    /// </summary>
    public virtual ICollection<Userscardset> Userscardsets { get; set; } = new List<Userscardset>();
}
