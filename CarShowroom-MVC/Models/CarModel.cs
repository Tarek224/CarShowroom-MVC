using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarShowroom.Models
{
    public enum Color
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 4,
        Black = 8,
        White = 16
    }
    [Table(name: "Category")]
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Category_ID { get; set; }

        [Required, MaxLength(50)]
        public string Category_Name { get; set; }

        public string Category_Describtion { get; set; }

        public virtual List<Car> Cars { get; set; }
    }

    [Table(name: "Car")]
    public class Car
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Car_ID { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int? Category_ID { get; set; }

        public string Description { get; set; }

        public Color color { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Acceleration { get; set; }

        public int Stored_Quantity { get; set; }

        [ForeignKey("Category_ID")]
        public virtual Category Category { get; set; }

        public virtual List<WishList> WishLists { get; set; }
        public virtual List<CarImage> CarImages { get; set; }
        public virtual List<Comment> Comments { get; set; }

    }

    [Table(name: "CarImage")]
    public class CarImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int Car_ID { get; set; }

        [Required]
        public string Image { get; set; }

        [ForeignKey("Car_ID")]
        public virtual Car Car { get; set; }
    }

    [Table(name: "WishList")]
    public class WishList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Customer_ID { get; set; }

        public int Car_ID { get; set; }

        [ForeignKey("Car_ID")]
        public virtual Car Car { get; set; }
    }


    [Table(name: "Comment")]
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Customer_ID { get; set; }

        public int Car_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Comment_Date { get; set; }

        [ForeignKey("Customer_ID")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Car_ID")]
        public virtual Car Car { get; set; }

    }
}