INCLUDE globalVars.ink

{visitedShop == false: -> FirstVisit}
{allItemsBought == false: -> RegularVisit | -> AllItemsBought}

= FirstVisit //Maybe revise
-Dialogue meeting the shop keeper for the first time-
~ visitedShop = true
-> END

= RegularVisit
Hey welcome back! Any items you want to know more about?
+ [About Digging Tools...]
    -> AboutDiggingTools
+ [About Metal Detectors...]
    -> AboutMetalDetectors
+ [Nope.]
    Right then, let me know if you need anything.
    -> END
-> END

= AllItemsBought
-Dialogue about going out of business-
-> END


= AboutDiggingTools
-Dialogue about buying and upgrading tools-
-> DONE

= AboutMetalDetectors
-Dialogue about buying and upgrading tools-
-> DONE

= RandomizedGreeting
-Random greetings, or greetings based off num of visits?-
-> DONE