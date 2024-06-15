using UnityEngine;


public class PlayerModel : CharacterModel<PlayerState>
{
    private PlayerController player;
    
    public override void Init(CharacterController<PlayerState> character)
    {
        base.Init(character);
        player = character as PlayerController;
    }

    protected override void OnSkillOver()
    {
        Debug.Log("OnSkillOver");
    }

}
