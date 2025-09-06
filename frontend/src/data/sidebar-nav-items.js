export default function() {
  return [
    // === BUSINESS SYSTEM ===
    {
      title: "Business Dashboard",
      to: "/dashboard",
      htmlBefore: '<i class="fas fa-tachometer-alt"></i>',
      htmlAfter: ""
    },
    {
      title: "User Management",
      htmlBefore: '<i class="fas fa-users"></i>',
      to: "/users",
    },
    {
      title: "Organizations",
      htmlBefore: '<i class="fas fa-building"></i>',
      to: "/organizations",
    },
    
    // === DIVIDER ===
    {
      title: "Legacy System",
      htmlBefore: '<i class="fas fa-folder"></i>',
      to: "#",
      htmlAfter: '<small class="text-muted ml-2">(Original Template)</small>'
    },
    
    // === LEGACY SHARDS TEMPLATE ===
    {
      title: "Blog Dashboard",
      to: "/blog-overview",
      htmlBefore: '<i class="fas fa-edit"></i>',
      htmlAfter: ""
    },
    {
      title: "Blog Posts",
      htmlBefore: '<i class="fas fa-list"></i>',
      to: "/blog-posts",
    },
    {
      title: "Add New Post",
      htmlBefore: '<i class="fas fa-plus"></i>',
      to: "/add-new-post",
    },
    {
      title: "Forms & Components",
      htmlBefore: '<i class="fas fa-th"></i>',
      to: "/components-overview",
    },
    {
      title: "Tables",
      htmlBefore: '<i class="fas fa-table"></i>',
      to: "/tables",
    },
    {
      title: "User Profile",
      htmlBefore: '<i class="fas fa-user"></i>',
      to: "/user-profile-lite",
    },
    {
      title: "Errors",
      htmlBefore: '<i class="fas fa-exclamation-triangle"></i>',
      to: "/errors",
    }
  ];
}
