﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.DTO.Note
{
    public class NoteRequestDto
    {
        /// <summary>
        /// Gets or sets the title of the note.
        /// The title is required and cannot exceed 200 characters.
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the note.
        /// The content is required and must be provided.
        /// </summary>
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the color of the note for display purposes.
        /// The color is optional but cannot exceed 50 characters.
        /// </summary>
        [StringLength(50, ErrorMessage = "Color cannot exceed 50 characters")]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets whether the note is pinned for quick access.
        /// Default is false (not pinned).
        /// </summary>
        public bool IsPinned { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the note is archived.
        /// Default is false (not archived).
        /// </summary>
        public bool IsArchived { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the note is marked as deleted.
        /// Default is false (not deleted).
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Gets or sets the ID of the user who owns this note.
        /// This field links the note to a user.
        /// </summary>
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
    }
}
