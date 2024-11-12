using Microsoft.AspNetCore.Mvc;
using PusherServer;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly Pusher _pusher;

    public NotificationsController(Pusher pusher)
    {
        _pusher = pusher;
    }

    [HttpPost("sendtofirst")]
    public async Task<IActionResult> SendMessagetoTheFirst([FromBody] ChatMessage message)
    {
        // Trigger the event through Pusher
        await _pusher.TriggerAsync("my-channel", "my-event-one", new { message.User, message.Text });

        return Ok();
    }

    [HttpPost("sendtosecond")]
    public async Task<IActionResult> SendMessageToTheSecond([FromBody] ChatMessage message)
    {
        // Trigger the event through Pusher
        await _pusher.TriggerAsync("my-channel", "my-event-two", new { message.User, message.Text });

        return Ok();
    }

}

// the message Model
public class ChatMessage
{
    public string User { get; set; }
    public string Text { get; set; }
}
