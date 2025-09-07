// Notification API Service using GraphQL
import { graphqlClient } from '../../services/api/graphqlClient';

class NotificationAPI {
  constructor() {
    this.client = graphqlClient;
  }

  // Send notification
  async sendNotification(notificationData) {
    try {
      const query = `
        mutation SendNotification($notification: NotificationInput!) {
          NotificationService_postNotificationsControllerSendNotification(sendNotificationDto: $notification) {
            data {
              id
              userId
              title
              message
              type
              isRead
              createdAt
            }
            isSuccess
            message
          }
        }
      `;

      const response = await this.client.mutate(query, { notification: notificationData });
      const result = response.NotificationService_postNotificationsControllerSendNotification;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to send notification'
      };
    }
  }

  // Get notifications for user
  async getNotifications(userId = null) {
    try {
      const query = `
        query GetNotifications($userId: String) {
          NotificationService_getNotificationsControllerGetNotifications(userId: $userId) {
            data {
              id
              userId
              title
              message
              type
              isRead
              createdAt
            }
            isSuccess
            message
          }
        }
      `;

      const response = await this.client.query(query, { userId });
      const result = response.NotificationService_getNotificationsControllerGetNotifications;
      
      return {
        success: result.isSuccess,
        data: result.data || [],
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: [],
        message: error.message || 'Failed to get notifications'
      };
    }
  }

  // Mark notification as read
  async markAsRead(notificationId) {
    try {
      const query = `
        mutation MarkAsRead($id: String!) {
          NotificationService_patchNotificationsControllerMarkAsRead(id: $id) {
            data
            isSuccess
            message
          }
        }
      `;

      const response = await this.client.mutate(query, { id: notificationId });
      const result = response.NotificationService_patchNotificationsControllerMarkAsRead;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to mark notification as read'
      };
    }
  }

  // Delete notification
  async deleteNotification(notificationId) {
    try {
      const query = `
        mutation DeleteNotification($id: String!) {
          NotificationService_deleteNotificationsControllerDeleteNotification(id: $id) {
            data
            isSuccess
            message
          }
        }
      `;

      const response = await this.client.mutate(query, { id: notificationId });
      const result = response.NotificationService_deleteNotificationsControllerDeleteNotification;
      
      return {
        success: result.isSuccess,
        data: result.data,
        message: result.message
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: error.message || 'Failed to delete notification'
      };
    }
  }
}

export default new NotificationAPI();