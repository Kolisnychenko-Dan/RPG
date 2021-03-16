namespace RPG.Controller
{
    public interface IRayCastable
    {
        bool HandleRaycast(PlayerController controllerToHandle);
        CursorType GetHoverCursor();
    }

}
