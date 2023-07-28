INCLUDE globalVars.ink

{visitedCurator == false: -> FirstVisit}
{artifactDonated == false: -> VisitsPreDonation | -> VisitsPostDonation}
{allArtifactsDonated: -> FullyDonated}

= FirstVisit //Maybe revise
Oh hi there! You must be here to deliver the artifacts correct?
No..?
...
Well great, then it seems we'll have to make do with NO artifacts.
It makes it hard to call ourselves a Museum, and makes it even harder to get people to stop in...
Even one item would be bound to get us some attention over here...
...
OH I'm sorry! Talking to myself over here in front of a visitor, not a good look for this place either, huh...
Thanks for stopping in anyways! Although we're still empty you can still take a look around if you'd like.
Enjoy your visit!
~ visitedCurator = true

-> END

= VisitsPreDonation
Hey again! We're still empty, but have fun looking around.
Did you want to ask about anything?
+ [About the Museum...]
    -> AboutTheMuseum
+ [About Donating...]
    -> DONE
+ [Just saying hi.]
    Well hi there to you too!
    And make sure to come back soon!
    -> END
-> END


= VisitsPostDonation
Hey again! We're still empty, but have fun looking around.
Did you want to ask about anything?
+ [About the Museum...]
    -> AboutTheMuseum
+ [About Donating...]
    -> DONE
+ [Just saying hi.]
    Well hi there to you too!
    And make sure to come back soon!
    -> END
-> END


= AboutTheMuseum
//Triggers between 0 and 4 Donations
This Museum finished construction a few years ago, and was quite popular for a time.
Only recently did the artifacts start to go missing.
First we thought it was theft, but our security team couldn't catch a thing AND artifacts were still disappearing under their watch!
Then we hired another team, in case the first team was also part of the problem, but still the pieces vanished.
Finally we ordered some new artifacts to be delivered, so it at least didn't look as empty, but they've yet to arrive...
-> DONE

= AboutTheMuseumPart2
//Triggers after ~5 artifacts have been donated
-> DONE

= AboutTheMuseumPart3
//Triggers after all artifacts have been donated
-> DONE

= AboutDonating
-Dialogue about donation process-
-> DONE

= FullyDonated
-Dialogue about finishing the museum-
-> END