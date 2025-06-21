using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Orange_tree_editor.Models;

public class Blog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    [BsonElement("blog_slug")]
    public string BlogSlug { get; set; }
    
    public string Content { get; set; }
    
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }
}