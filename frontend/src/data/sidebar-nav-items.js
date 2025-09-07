import { menuApi } from '../api';

// Dynamic menu loader using GraphQL API
export default async function() {
  // Get company code from URL or localStorage
  const getCompanyCode = () => {
    const path = window.location.pathname;
    const match = path.match(/^\/([^/]+)/);
    if (match && match[1] !== 'company' && match[1] !== 'login') {
      return match[1];
    }
    return localStorage.getItem('companyCode') || 'demo';
  };
  
  const companyCode = getCompanyCode();
  
  // Try to load menu from API
  try {
    const menuResult = await menuApi.getMenuTree('main');
    if (menuResult.success && menuResult.data.length > 0) {
      // Transform API menu to sidebar format
      return menuResult.data.map(item => ({
        title: item.title,
        to: item.to ? `/${companyCode}${item.to}` : '#',
        htmlBefore: `<i class="${item.icon || 'fas fa-circle'}"></i>`,
        htmlAfter: '',
        items: item.items ? item.items.map(subItem => ({
          title: subItem.title,
          to: `/${companyCode}${subItem.to}`,
          htmlBefore: `<i class="fas fa-angle-right"></i>`
        })) : undefined
      }));
    }
  } catch (error) {
    console.warn('Failed to load menu from API, using fallback:', error);
  }
  
  // Fallback static menu
  return [
    // === BUSINESS SYSTEM ===
    {
      title: "Business Dashboard",
      to: `/${companyCode}/dashboard`,
      htmlBefore: '<i class="fas fa-tachometer-alt"></i>',
      htmlAfter: ""
    },
    
    // === ORGANIZATION (EXPANDABLE) ===
    {
      title: "Organization",
      htmlBefore: '<i class="fas fa-building"></i>',
      to: "#",
      items: [
        {
          title: "User Management",
          htmlBefore: '<i class="fas fa-users"></i>',
          to: `/${companyCode}/users`,
        },
        {
          title: "Role Management",
          htmlBefore: '<i class="fas fa-shield-alt"></i>',
          to: `/${companyCode}/roles`,
        },

        {
          title: "Departments",
          htmlBefore: '<i class="fas fa-sitemap"></i>',
          to: `/${companyCode}/departments`,
        },
        {
          title: "Positions",
          htmlBefore: '<i class="fas fa-briefcase"></i>',
          to: `/${companyCode}/positions`,
        }
      ]
    },
    
    // === SYSTEM SETTINGS (EXPANDABLE) ===
    {
      title: "System Settings",
      htmlBefore: '<i class="fas fa-cogs"></i>',
      to: "#",
      items: [
        {
          title: "Company Info",
          htmlBefore: '<i class="fas fa-building"></i>',
          to: `/${companyCode}/company-profile`,
        },
        {
          title: "Menu Tree",
          htmlBefore: '<i class="fas fa-sitemap"></i>',
          to: `/${companyCode}/menu-tree`,
        }
      ]
    },
    
    // === LEGACY SYSTEM (EXPANDABLE) ===
    {
      title: "Legacy System",
      htmlBefore: '<i class="fas fa-archive"></i>',
      to: "#",
      htmlAfter: '<small class="text-muted ml-2">(Original Template)</small>',
      items: [
        {
          title: "Blog Dashboard",
          htmlBefore: '<i class="fas fa-edit"></i>',
          to: `/${companyCode}/legacy-system/blog-dashboard`,
        },
        {
          title: "Blog Posts",
          htmlBefore: '<i class="fas fa-list"></i>',
          to: `/${companyCode}/legacy-system/blog-posts`,
        },
        {
          title: "Add New Post",
          htmlBefore: '<i class="fas fa-plus"></i>',
          to: `/${companyCode}/legacy-system/add-new-post`,
        },
        {
          title: "Forms & Components",
          htmlBefore: '<i class="fas fa-th"></i>',
          to: `/${companyCode}/legacy-system/forms-components`,
        },
        {
          title: "Tables",
          htmlBefore: '<i class="fas fa-table"></i>',
          to: `/${companyCode}/legacy-system/tables`,
        },
        {
          title: "User Profile",
          htmlBefore: '<i class="fas fa-user"></i>',
          to: `/${companyCode}/legacy-system/user-profile`,
        },
        {
          title: "Errors",
          htmlBefore: '<i class="fas fa-exclamation-triangle"></i>',
          to: `/${companyCode}/legacy-system/errors`,
        }
      ]
    }
  ];
}
