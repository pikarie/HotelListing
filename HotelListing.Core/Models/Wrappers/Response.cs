namespace HotelListing.Core.Models.Wrappers
{
	/// <summary>
	///	Originally called QueryParameters in the Udemy course.
	///	Renamed after following this tutorial: https://codewithmukesh.com/blog/pagination-in-aspnet-core-webapi/
	/// </summary>
	public class Response<T>
	{
		public Response()
		{

		}

		public Response(T data)
		{
			Succeeded = true;
			Message = string.Empty;
			Errors = null;
			Data = data;
		}
		public T Data { get; set; }
		public bool Succeeded { get; set; }
		public string[] Errors { get; set; }
		public string Message { get; set; }
	}
}
