using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;

namespace Content.Client.Voting.UI;

public sealed class VoteUIController : UIController
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly IVoteManager _voteManager = default!;

    private VoteContainer? VoteContainer => UIManager.GetActiveUIWidgetOrNull<VoteContainer>();

    private readonly List<VoteManager.ActiveVote> _activeVotes = new();
    private readonly VoteManager.ActiveVote _vote;
    private readonly Button[] _voteButtons;

    public override void Initialize()
    {
    }
    
    public VoteUIController()
    {
        _channelSelectorPopup = UIManager.CreatePopup<VotePopup>();
        
        // TODO: initalize _vote
        
        _voteButtons = new Button[vote.Entries.Length];
        var group = new ButtonGroup();

        for (var i = 0; i < _voteButtons.Length; i++)
        {
            var button = new Button
        {
                ToggleMode = true,
                Group = group
            };
            _voteButtons[i] = button;
            VoteOptionsContainer.AddChild(button);
            var i1 = i;
            button.OnPressed += _ => _voteManager.SendCastVote(vote.Id, i1);
        }
    }

    public void AddVotePopup()
    {
        if (VoteContainer == null)
            return;
        var votePopup = new VotePopup();
        
        VoteContainer.AddChild(votePopup);
    }

    public void UpdateData()
    {
        VoteTitle.Text = _vote.Title;
        VoteCaller.Text = Loc.GetString("ui-vote-created", ("initiator", _vote.Initiator));

        for (var i = 0; i < _voteButtons.Length; i++)
        {
            var entry = _vote.Entries[i];
            _voteButtons[i].Text = Loc.GetString("ui-vote-button", ("text", entry.Text), ("votes", entry.Votes));

            if (_vote.OurVote == i)
                _voteButtons[i].Pressed = true;
        }
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        // Logger.Debug($"{_gameTiming.ServerTime}, {_vote.StartTime}, {_vote.EndTime}");

        var curTime = _gameTiming.RealTime;
        var timeLeft = _vote.EndTime - curTime;
        if (timeLeft < TimeSpan.Zero)
            timeLeft = TimeSpan.Zero;

        // Round up a second.
        timeLeft = TimeSpan.FromSeconds(Math.Ceiling(timeLeft.TotalSeconds));

        TimeLeftBar.Value = Math.Min(1, (float) ((curTime.TotalSeconds - _vote.StartTime.TotalSeconds) /
                                                 (_vote.EndTime.TotalSeconds - _vote.StartTime.TotalSeconds)));

        TimeLeftText.Text = $"{timeLeft:m\\:ss}";
    }
}