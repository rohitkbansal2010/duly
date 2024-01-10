
export type AllergieCategoryType = 'Food' | 'Medication' | 'Biologic' | 'Environment' | 'Other';

export type AllergieReactionSeverityType = 'Severe' | 'Moderate' | 'Mild';

export type AllergieReactionType = {
  title: string;
  severity: AllergieReactionSeverityType;
};

export type AllergiesData = {
  id: string;
  title?: string;
  recorded: string;
  categories: AllergieCategoryType[];
  reactions: AllergieReactionType[];
}
