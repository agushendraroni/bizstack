// Menu API Service using GraphQL
import { graphqlClient } from '../../services/api/graphqlClient';

class MenuAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Get menu tree structure
  async getMenuTree(context = 'main') {
    try {
      // Static menu structure for now - can be extended with GraphQL later
      const menuTree = [
        {
          id: 1,
          title: 'Dashboard',
          icon: 'fas fa-tachometer-alt',
          to: '/dashboard',
          context: 'main'
        },
        {
          id: 2,
          title: 'Products',
          icon: 'fas fa-box',
          to: '/products',
          context: 'main'
        },
        {
          id: 3,
          title: 'Customers',
          icon: 'fas fa-users',
          to: '/customers',
          context: 'main'
        },
        {
          id: 4,
          title: 'Orders',
          icon: 'fas fa-shopping-cart',
          to: '/orders',
          context: 'main'
        },
        {
          id: 5,
          title: 'Reports',
          icon: 'fas fa-chart-bar',
          to: '/reports',
          context: 'main'
        },
        {
          id: 6,
          title: 'Organization',
          icon: 'fas fa-building',
          to: '#',
          context: 'main',
          items: [
            {
              id: 61,
              title: 'User Management',
              to: '/users'
            },
            {
              id: 62,
              title: 'Role Management',
              to: '/roles'
            },
            {
              id: 63,
              title: 'Departments',
              to: '/departments'
            },
            {
              id: 64,
              title: 'Positions',
              to: '/positions'
            }
          ]
        },
        {
          id: 7,
          title: 'Settings',
          icon: 'fas fa-cogs',
          to: '#',
          context: 'main',
          items: [
            {
              id: 71,
              title: 'Company Profile',
              to: '/company-profile'
            },
            {
              id: 72,
              title: 'Menu Management',
              to: '/menu-tree'
            }
          ]
        }
      ];

      return {
        success: true,
        data: menuTree,
        message: 'Menu tree retrieved successfully'
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch menu tree'
      };
    }
  }

  // Get flat menu structure
  async getMenuFlat(context = 'main') {
    try {
      const treeResult = await this.getMenuTree(context);
      
      if (treeResult.success) {
        const flatMenu = [];
        
        const flattenMenu = (items, parentId = null) => {
          items.forEach(item => {
            flatMenu.push({
              ...item,
              parentId
            });
            
            if (item.items) {
              flattenMenu(item.items, item.id);
            }
          });
        };
        
        flattenMenu(treeResult.data);
        
        return {
          success: true,
          data: flatMenu,
          message: 'Flat menu retrieved successfully'
        };
      }
      
      return treeResult;
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch flat menu'
      };
    }
  }

  // Get main menu items
  async getMainMenuItems() {
    try {
      const result = await this.getMenuTree('main');
      return result;
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to fetch main menu items'
      };
    }
  }
}

export default new MenuAPI();