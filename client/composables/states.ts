export const useSeason = () => useState<Number>('season', () => 7)
export const usePlayoffs = () => useState<Boolean>('playoffs', () => false)