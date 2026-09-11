namespace TRG_Markets.Domain.Entities;

public enum ProfitLightState { Black, Grey, Red, Orange, Amber, Blue, Green, BrightGreen }
public enum ProfitLightAction { Unavailable, Abstain, Reject, Protect, Wait, Enter }
public enum ProfitLightConfidence { None, Low, Medium, High }
public enum ProfitLightSafetyDecision { Allow, Reject, Emergency }
