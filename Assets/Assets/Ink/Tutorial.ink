VAR class = ""
VAR item = ""
VAR cost = ""
/* _________________________________ */

-> main

=== main ===
You new here? # speaker: narrator
    *[Yes.]
    *[Apparently.]
    *[No, I just wanted to replay the tutorial.]
        So, either you're the person who's trying to mark this project, or a beloved friend the dev talked into testing it ... or you're just forgetful.
        * * [Person who's trying to mark this project.]
            Please imagine me in a suit and a proper british accent then.
            Shall we begin?
        * * [Beloved friend or something alike.]
            Message from the dev:
            You are loved and I am eternal grateful to know you <3
            Let's get this started.
        * * [Just forgetful.]
            Alrighty tighty, then once again: # background: village
    - The reasons you choose this life can be many.  
Maybe you're just looking for honor, riches or you like being stabbed repeatedly with various weapons.
Adventure does not discriminate.
-> characterselection

=== characterselection ===
What is your background?
    + [Fighter]
        ~ class = "fighter"
        You fight on the frontlines, your weapons of choice is a sword or an axe.
        -> acceptSelectedCharacter
    + [Thief]
        ~  class = "thief"
        You like to deceive and the thrill of stealing. In combat you prefer to stay hidden. Your weapon of choice is a dagger or a bow.
        -> acceptSelectedCharacter
    + [Sorcerer]
        ~ class = "sorcerer"
        You wield great power, but it comes with a price. Your weapon of choice, beside your magic, is your staff.
        -> acceptSelectedCharacter

=== acceptSelectedCharacter ===
Does this sound like you?
    * [Yes. I am a {class}.]
    -> startAdventure
    + [No, that does not feel right.]
    -> characterselection
    
=== startAdventure ===
This city may be new to you, but not the smell.
It's the unmistakable stench of people going about their day, various animals with their excretions and stone baked in the sun.
You just stared at a wooden board in the middle of the market place, looking for some work.
Or better, looking for some coins so you can get proper food and a nice bed for the night.
Some tavernkeeper is looking for help to remove the rats from their basement.


{class == "fighter": It's good, honest work and you can help someone.}
{class == "thief": And maybe you can find interesting things in the basement for your collection.}
{class == "sorcerer": Just don't try to throw fireballs at them and you should be fine and set up for the night.}

But first, you should stop by the local shop.
-> firstTimeShopper

=== firstTimeShopper ===
Placeholdertext. #TODO
-> shopping

=== shopping ===
Items you can purchase:
    + I'm done shopping.
            Are you sure? You won't be able to come back until you completed the tutorial.
            *** Yes.
            -> firstTimeShopperEnd
            *** No.
            -> firstTimeShopper
    + [Potion of Healing]
        ~ item = "Potion of Healing"
        ~ cost = "50"
    + [Cheese]
        ~ item = "cheese"
        ~ cost = "3"
    + [Sharp Sword]
        ~ item = "sharp sword"
        ~ cost = "75"
    + [Potion of Magic]
        ~ item = "Potion of Magic"
        ~ cost = "75"

    - The {item} will cost you {cost}G.
        ++ Buy it. # TODO add logic
            You bought {item} for {cost}G.
            -> firstTimeShopper
        ++ [Don't buy it.]
            -> firstTimeShopper

        
=== firstTimeShopperEnd ===
After completing your shopping, the shopkeeper is turning their back to you.
On the table is a weird looking potion.
-> stealOpportunity

=== stealOpportunity ===
Do you want to steal the weird potion?
    * [Yes.]
    -> stealWeirdPotion
    * [No.]
    -> DONE
    
=== stealWeirdPotion ===
// logic in code :)
    * You were caught.
        ** {class == "thief"} You could deceive them.
            Silver Tongue and sticky hands, a combination designated for greatness.
            Or prison.
            -> toTheTavern
        ** {class == "thief"} You couldn't deceive them.
            It seems the more you tried to weasel your way out of it, the more the shopkeeper got angry.
            You really should avoid them for a bit.
            -> toTheTavern
        ** {class == "thief"} You didn't try to deceive them.
            Well, you shouldn't come back to this shop in the near future.
            -> toTheTavern
        ** Well, you shouldn't come back to this shop in the near future.
            -> toTheTavern
    * You weren't caught.
            Model citizen at work.
            -> toTheTavern
            
=== toTheTavern ==
#speaker: narrator
The next stop is the tavern.
As this is a very small city, it is right across the town square.
Your eyes need a few seconds to adjust to the candle-light room.
In the far corner, you can see the last embers of a fire long forgotten.
The people you can see are not directly looking at you, but you can feel their eyes following every move you make.
Behind the bar is a short, bearded person wiping away at a dirty glass.
You are not sure if their goal is to dry it off or to smudge the stains on it more.
You approach them.
Yes? #speaker: tavernkeeper
-> theTavernkeeper

=== theTavernkeeper ===
    * [Give them the piece of paper they put up on the quest board.]
        -> newHelp
    * [Tell them you're here because of the rats.]
        Rats? #speaker: tavernkeeper
        Oh no, you're mistaken, we have no rats here.
        They jump up onto the bar, their hand clamping hard on your shoulder. #speaker: narrator
        -> newHelp
    * [Study their technique with the dirty glass. ]
        It's remarkable. #speaker: narrator
        With every movement of their wrist, they are able to incorporate more and more dirt into the glass.
        You ask yourself if someone trained them or if there are some Olympics of Barkeepres.
        This guy for sure could win it.
        If the category is 'making sure I never want to drink here'.
        ->  theTavernkeeper
    

=== newHelp ===
You must be the new help for our kitchen, why don't you come to the back. #speaker: tavernkeeper
-> inTheBack

=== inTheBack ===
As you don't feel you have any other choice, you follow them to the back. #speaker: narrator
You're standing in a little corridor, with two other doors and some stairs, presumly to the basement.
We are a very clean establishment, so there are DEFINITLY no rats in our basement.
    * Point to the paper. # if paper given TODO
    * Nod slowy and knowingly.
    * Stare at them.
    - So I just want you to, eh, look at our basement.
And if you see something ratshaped there, well, as I said:
There are DEFINITLY no rats in the basement.
And after you looked at our basement, and i confirm that it is STILL ratfree, you get your reward for, eh, looking at our basement.
    * [Point to the paper.] # if paper given TODO
    * [Nod slowy and knowingly.]
    * [Stare at them.]
    - Yes, yes, and as a reward ...
You're new here in town, do you have somwhere to rest?
 /*   * Yes.
        Well, that's for sure a lie. */
    * [No.]
    - I am far from rich, but what if you can make yourself a home behind some crates?
    Nothing fancy, but better than the streets.
    Some coin wuld be nice, but a warm place sounds good, especially after your last weeks on the road.
    Alright, here's to the basement ...
    I will come down in half an hour.
    Good luck.
    -> theBasement
    
=== theBasement ===
They carefully open the door and, with a swift motion, push you into the room.
You hear the rustling of keys and something softly clicking-
Did they just ... lock you in?
    * [Punch against the door.]
        Great. #TODO logic
        Now you're locked in AND your fist is aching.
        -> ratIsAttacking
    * [Shout at them.]
        Nothing.
        Nobody will come to your help
        You and your dreams will die here and ...
        -> ratIsAttacking
    * [Look at your environment.]
        There is some sort of arcane light under the ceiling, but it's dim and flickering.
        You can see some crates and barrels.
        Beside a torn up bag is a giant rat, munching away at it's former contet.
        -> youAreAttacking

=== ratIsAttacking ===
What's that?
You heard something.
Right.
Behind.
You.
-> Fight //rat will go first

=== youAreAttacking ===
Time to fight.
-> Fight // you will go first

=== Fight ===
-> DONE


/*

    {hasEnoughFood} [{ShowFoodChoice(0, -10, "eat food")}]

    {AlterFood(-10)}



*/





    
    
    
    
    
    
-> END
        