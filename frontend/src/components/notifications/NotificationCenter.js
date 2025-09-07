import React, { useState, useEffect } from "react";
import { Badge, Button } from "shards-react";
import notificationApi from "../../api/notification/notificationApi";

const NotificationCenter = ({ userId, onClose }) => {
  const [notifications, setNotifications] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadNotifications();
  }, [userId]);

  const loadNotifications = async () => {
    try {
      setLoading(true);
      const result = await notificationApi.getNotifications(userId);
      
      if (result.success) {
        setNotifications(result.data || []);
      }
    } catch (err) {
      console.error("Error loading notifications:", err);
    } finally {
      setLoading(false);
    }
  };

  const handleMarkAsRead = async (notificationId) => {
    try {
      const result = await notificationApi.markAsRead(notificationId);
      if (result.success) {
        setNotifications(notifications.map(n => 
          n.id === notificationId ? { ...n, isRead: true } : n
        ));
      }
    } catch (err) {
      console.error("Error marking notification as read:", err);
    }
  };

  const unreadCount = notifications.filter(n => !n.isRead).length;

  return (
    <div className="notification-center" style={{ 
      position: 'absolute', 
      top: '100%', 
      right: 0, 
      width: '350px', 
      maxHeight: '400px', 
      zIndex: 1000,
      backgroundColor: 'white',
      border: '1px solid #dee2e6',
      borderRadius: '0.375rem',
      boxShadow: '0 0.5rem 1rem rgba(0, 0, 0, 0.15)'
    }}>
      <div className="p-3 border-bottom d-flex justify-content-between align-items-center">
        <h6 className="mb-0">
          Notifications 
          {unreadCount > 0 && (
            <Badge theme="danger" className="ml-2">{unreadCount}</Badge>
          )}
        </h6>
        <Button theme="light" size="sm" onClick={onClose}>
          <i className="fas fa-times"></i>
        </Button>
      </div>
      
      <div style={{ maxHeight: '300px', overflowY: 'auto' }}>
        {loading ? (
          <div className="text-center p-3">Loading...</div>
        ) : notifications.length === 0 ? (
          <div className="text-center p-3 text-muted">No notifications</div>
        ) : (
          notifications.map(notification => (
            <div 
              key={notification.id} 
              className={`p-3 border-bottom ${!notification.isRead ? 'bg-light' : ''}`}
            >
              <div className="d-flex justify-content-between">
                <div>
                  <h6 className="mb-1" style={{ fontSize: '0.875rem' }}>
                    {notification.title}
                  </h6>
                  <p className="mb-1 text-muted" style={{ fontSize: '0.75rem' }}>
                    {notification.message}
                  </p>
                  <small className="text-muted">
                    {new Date(notification.createdAt).toLocaleString()}
                  </small>
                </div>
                {!notification.isRead && (
                  <Button 
                    theme="link" 
                    size="sm" 
                    onClick={() => handleMarkAsRead(notification.id)}
                  >
                    <i className="fas fa-check"></i>
                  </Button>
                )}
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
};

export default NotificationCenter;