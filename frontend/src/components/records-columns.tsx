import type { RecordsDto } from "@/types/records"
import type { ColumnDef } from "@tanstack/react-table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Button } from "@/components/ui/button"
import { MoreHorizontal } from "lucide-react"
import { record } from "@/lib/api";

import DialogDelete from "@/features/dialog-delete";
import EditRecordDialog from "@/features/records/dialog-edit";
import { useState } from "react";

function deleteRecord(recordId: number): Promise<boolean> {
  return record.delete(recordId)
}

export const columns: ColumnDef<RecordsDto>[] = [
  {
    accessorKey: "recordName",
    header: "Name",
  },
  {
    accessorKey: "target",
    header: "Target",
  },
  {
    accessorKey: "priority",
    header: "Priority",
  },
  {
    accessorKey: "ttl",
    header: "TTL",
  },
  {
    accessorKey: "recordType",
    header: "Type",
  },
  {
    id: "actions",
    cell: ({ row }) => {
        const recordItem = row.original
        const [showEditDialog, setShowEditDialog] = useState(false)
        const [isDeleteOpen, setIsDeleteOpen] = useState(false)

        return (
        <>
          <DropdownMenu>
            <DropdownMenuTrigger asChild className="absolute right-0 transform -translate-y-1/2">
              <Button variant="ghost" className="h-8 w-8 p-0">
                <span className="sr-only">Open menu</span>
                <MoreHorizontal className="h-4 w-4" />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end" onCloseAutoFocus={(e) => e.preventDefault()}>
              <DropdownMenuLabel>Actions</DropdownMenuLabel>
              <DropdownMenuSeparator />
              
              {/* Edit Button */}
              <DropdownMenuItem 
                className="cursor-pointer p-0"
                onSelect={(e) => e.preventDefault()}
              >
                <div className="w-full">
                  <EditRecordDialog
                    recordId={recordItem.id}
                    open={showEditDialog}
                    onOpenChange={setShowEditDialog}
                  />
                </div>
              </DropdownMenuItem>
              
              {/* Delete Button */}
              <DropdownMenuItem 
                className="cursor-pointer p-0 text-red-600"
                onSelect={(e) => e.preventDefault()}
              >
                <div className="w-full">
                  <DialogDelete 
                    message="record" 
                    onSubmit={() => deleteRecord(recordItem.id)}
                    open={isDeleteOpen}
                    onOpenChange={setIsDeleteOpen}
                  />
                </div>
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </>
        )
    },
  }
]