namespace SharedLibrary.Constants;

public enum TaskEventType
{
    Created,            // Создание задачи
    Assigned,           // Назначен исполнитель
    StatusChanged,      // Изменился статус
    CommentAdded,       // Добавлен комментарий
    DocumentAttached    // Прикреплен документ
} 