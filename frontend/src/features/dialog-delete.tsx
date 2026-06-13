import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import { Trash } from "lucide-react"
import { useState } from "react"
import Spinner from "@/components/ui/spinner"

interface DialogDeleteProps {
  onSubmit: () => Promise<boolean>
  message: string
  open?: boolean
  onOpenChange?: (open: boolean) => void
}

export default function DialogDelete({
  onSubmit,
  message,
  open: controlledOpen,
  onOpenChange
}: DialogDeleteProps) {
  const [isLoading, setIsLoading] = useState(false)

  const handleDelete = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setIsLoading(true)

    try {
      const success = await onSubmit()

      if (success) {
        if (onOpenChange) {
          onOpenChange(false)
        }
      }
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <>
      {/* Custom button to open dialog */}
      <Button
        className="w-full justify-start text-red-600 hover:text-red-700" 
        variant="ghost"
        onClick={() => onOpenChange && onOpenChange(true)}
      >
        <Trash className="mr-2 h-4 w-4" />
        Delete {message}
      </Button>
      
      {/* Dialog */}
      <Dialog open={controlledOpen} onOpenChange={onOpenChange}>
        <DialogContent className="sm:max-w-[425px]" onInteractOutside={(e) => e.preventDefault()}>
          <form onSubmit={handleDelete}>
            <DialogHeader>
              <DialogTitle>Delete {message}</DialogTitle>
              <DialogDescription>
                Are you sure you want to delete this {message}?
              </DialogDescription>
            </DialogHeader>
            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                onClick={() => onOpenChange && onOpenChange(false)}
              >
                Cancel
              </Button>
              <Button type="submit" disabled={isLoading} variant="destructive">
                {isLoading ? <Spinner /> : "Delete"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </>
  )
}