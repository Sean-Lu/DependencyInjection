using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Example.Domain.Entities
{
    /// <summary>
    /// 测试表
    /// </summary>
    [Table("Test", Schema = "public")]
    public class TestEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}