﻿using System;
using System.Collections.Generic;

namespace RhymeBinder.Models.DBModels
{
    public partial class SimpleUser
    {
        public SimpleUser()
        {
            BinderCreatedByNavigations = new HashSet<Binder>();
            BinderLastModifiedByNavigations = new HashSet<Binder>();
            BinderUsers = new HashSet<Binder>();
            EditWindowProperties = new HashSet<EditWindowProperty>();
            GroupHistories = new HashSet<GroupHistory>();
            SavedViews = new HashSet<SavedView>();
            TextGroups = new HashSet<TextGroup>();
            TextHeaderCreatedByNavigations = new HashSet<TextHeader>();
            TextHeaderLastModifiedByNavigations = new HashSet<TextHeader>();
            TextHeaderLastReadByNavigations = new HashSet<TextHeader>();
            TextHeaderVisionCreatedByNavigations = new HashSet<TextHeader>();
            TextRecords = new HashSet<TextRecord>();
            GranteeSharedObjects = new HashSet<SharedObjects>();
            GrantorSharedObjects = new HashSet<SharedObjects>();

        }

        public int UserId { get; set; }
        public string AspNetUserId { get; set; }
        public string UserName { get; set; }
        public int DefaultRecordsPerPage { get; set; }
        public bool DefaultShowLineCount { get; set; }
        public bool DefaultShowParagraphCount { get; set; }
        public int TimeZone { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ICollection<Binder> BinderCreatedByNavigations { get; set; }
        public virtual ICollection<Binder> BinderLastModifiedByNavigations { get; set; }
        public virtual ICollection<Binder> BinderUsers { get; set; }
        public virtual ICollection<EditWindowProperty> EditWindowProperties { get; set; }
        public virtual ICollection<GroupHistory> GroupHistories { get; set; }
        public virtual ICollection<SavedView> SavedViews { get; set; }
        public virtual ICollection<TextGroup> TextGroups { get; set; }
        public virtual ICollection<TextHeader> TextHeaderCreatedByNavigations { get; set; }
        public virtual ICollection<TextHeader> TextHeaderLastModifiedByNavigations { get; set; }
        public virtual ICollection<TextHeader> TextHeaderLastReadByNavigations { get; set; }
        public virtual ICollection<TextHeader> TextHeaderVisionCreatedByNavigations { get; set; }
        public virtual ICollection<TextRecord> TextRecords { get; set; }
        public virtual ICollection<Shelf> Shelves { get; set; }
        public virtual ICollection<SharedObjects> GrantorSharedObjects { get; set; }
        public virtual ICollection<SharedObjects> GranteeSharedObjects { get; set; }
    }
}
